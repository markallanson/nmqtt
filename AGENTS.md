# AGENTS.md

Guidance for AI coding agents working in this repository.

## What this is

nMQTT is a **legacy .NET Framework 4.0 Client Profile** MQTT v3.1 client library (C#) that
uses **Reactive Extensions (Rx)** for subscriptions. This repo is a read-only mirror of the
upstream [markallanson/nmqtt](https://github.com/markallanson/nmqtt). Code is circa 2013 and
uses the **old (non-SDK) MSBuild project format** (`ToolsVersion=4.0`). It is unmaintained.

## Toolchain — read this before running anything

- ❌ **Do not use `dotnet build` / `dotnet test` / `dotnet restore`.** This is a pre-SDK,
  .NET Framework 4.0 project. Those commands will not work.
- ✅ Use the classic **MSBuild** toolchain: `msbuild` on Windows (VS 2012+ or Build Tools with
  the v4.0 toolset) or `msbuild`/`xbuild` under **Mono** on Linux/macOS. The original CI ran on
  Ubuntu/Mono.
- The v4.0 targeting pack / reference assemblies must be available, or the build will fail to
  resolve `mscorlib` etc.
- NuGet packages are **gitignored** and must be restored before the first build. `NuGet.exe` is
  checked in at `.nuget/NuGet.exe` (run under `mono` on Linux).

## Commands (verified against the project files)

```bash
# Restore NuGet packages (checked-in NuGet.exe)
mono .nuget/NuGet.exe restore BuildWithTests.v11.sln      # Linux/Mono
.nuget/NuGet.exe restore BuildWithTests.v11.sln            # Windows

# Build — library only (fast feedback loop)
msbuild Build.v11.sln /p:Configuration=Debug

# Build — everything (library + tests + WinForms sample app)
msbuild BuildWithTests.v11.sln /p:Configuration=Release

# Run tests (build Debug first — the script expects nMQTTTests/bin/Debug/NmqttTests.dll)
msbuild BuildWithTests.v11.sln /p:Configuration=Debug
msbuild teamcity-tests.msbuild
# …or run the xUnit console runner directly (clr4 runner for .NET 4.0 tests):
mono packages/xunit.1.9.2/tools/xunit.console.clr4.exe nMQTTTests/bin/Debug/NmqttTests.dll
```

- **Build only what you need.** `Build.v11.sln` (library) is much faster than
  `BuildWithTests.v11.sln` (which also pulls in the WinForms sample app).
- **Lint = FxCop Code Analysis.** It runs **only in `Release`** (`RunCodeAnalysis=true`) and
  `TreatWarningsAsErrors=true` in **both** configurations. So: any warning breaks the build,
  and any FxCop violation breaks the Release build specifically. Prefer **Debug** for iteration.

## Repo layout

| Path | Purpose |
|---|---|
| `nMQTT/` | The client library. Root namespace `Nmqtt`. |
| `nMQTT/MqttClient.cs` | **Public entry point**: `Connect`, `ListenTo`, `PublishMessage`, `Dispose`. Start here. |
| `nMQTT/ConnectionHandling/` | TCP connection (`MqttConnection`), synchronous handler, 30 s keep-alive pings. |
| `nMQTT/Messages/` | One folder per MQTT message type. Each message has a base `.cs`, a `*.Fluent.cs` (builder API), and a variable-header/payload class. |
| `nMQTT/DataConverters/` | `IPayloadConverter<T>` interface + `AsciiPayloadConverter` / `PassThroughPayloadConverter`. |
| `nMQTT/SubscriptionsManager.cs` | The Rx subscription registry — lazy subscribe, `Publish().RefCount()`, auto-unsubscribe. |
| `nMQTT/PublishingManager.cs` | Outbound publish + inbound message dispatch to subscribers. |
| `nMQTT/Encoding/MqttEncoding.cs` | MQTT string (modified UTF-8 + length prefix) encode/decode. |
| `nMQTTTests/` | xUnit 1.9.2 tests. Mirrors the `Messages/` layout with `Serialization.cs` + `Deserialization.cs` per message type. Uses Moq. |
| `nMQTTTests/MockBroker.cs` | A minimal in-process TCP broker for connection/handler tests. |
| `nMqttSampleApp/` | WinForms sample app (connect/subscribe/publish GUI). Good real-usage reference — see `MqttHandler.cs`. |
| `TestBedSubscriber/` | ⚠️ Console sample that **does not compile** and is **not in any solution**. See gotchas. |
| `Build.v11.sln` | Library-only solution. |
| `BuildWithTests.v11.sln` | Library + tests + sample app. |
| `teamcity-tests.msbuild` | MSBuild script that runs the xUnit tests via an MSBuild task. |
| `getting-started.md` | Human-facing API quickstart. |

## Key conventions & gotchas

- **Rx subscription model is the central design decision.** `MqttClient.ListenTo(topic, qos)`
  returns `IObservable<MqttReceivedMessage<T>>`. Subscribing to the broker is **lazy** — the
  `SUBSCRIBE` packet is sent only on the first `.Subscribe(...)`. It is `Publish().RefCount()`,
  so the broker is **auto-unsubscribed when the last observer disposes**. When changing
  subscription code, preserve the refcount/lazy semantics.
- **Synchronous, one-way connection.** `Connect()` blocks until the broker ACKs (≤5 attempts,
  5 s each) or throws `ConnectionException`. There is **no reconnect**: create → connect → use
  → `Dispose()`. Do not add a reconnect loop without understanding this is intentional.
- **Inbound messages arrive on an indeterminate thread** (async `BeginRead` callback in
  `MqttConnection`). UI consumers must `.ObserveOn(...)`; sample code in `getting-started.md`
  and `nMqttSampleApp/MqttHandler.cs`.
- **QoS:** supported/published levels are **0** and **1**. `MqttQos.ExactlyOnce` (2) exists in
  the enum but is not part of the shipped feature set — don't assume QoS 2 works end-to-end.
- **Signed assemblies.** All projects sign with `nMqttDevOnly.snk` (checked in). Tests can see
  library internals via `InternalsVisibleTo` in `nMQTT/Properties/AssemblyInfo.cs` (keys are
  pinned for `NmqttTests`, `Moq`, and `DynamicProxyGenAssembly2`).
- **`TestBedSubscriber/Program.cs` is broken.** It calls `client.Observe(...)` (no such method;
  the real API is `ListenTo`) and has a corrupted `using System.Reactive.Linq.Observαble;`
  (a Greek alpha). It is **not** referenced by either `.sln`, so it won't break normal builds —
  leave it unless explicitly asked to fix it.
- **`getting-started.md` had stale type names** (`IPublishDataConverter`/`AsciiPublishDataConverter`
  → actually `IPayloadConverter`/`AsciiPayloadConverter`; `MqttRecivedMessage`/`MsqqReceivedMessage`
  → `MqttReceivedMessage`). These have been corrected; keep them corrected if you touch the file.
- **Case-sensitive filesystems.** Several commits fixed filename casing so the project builds on
  Linux/Mono. Preserve the exact casing in `.csproj` `<Compile Include="...">` entries and when
  adding files.
- **Don't "modernize" the project.** Upgrading to SDK-style `.csproj`, retargeting to .NET
  Core/5+, or switching to `async/await` is a large, out-of-scope migration, not a quick edit.
  Flag the desire to a human rather than doing it unprompted.

## Adding things

- **New MQTT message type:** create a folder under `nMQTT/Messages/<Name>/` with a base message
  class (deriving from `MqttMessage`), a `*.Fluent.cs` builder, and variable-header/payload
  classes; register it in `MqttMessageFactory`. Mirror the structure in `nMQTTTests/Messages/<Name>/`
  with `Serialization.cs` + `Deserialization.cs`.
- **New payload type:** implement `IPayloadConverter<T>` (must have a parameterless ctor; a single
  instance is cached and reused for all messages), then pass it as a type arg to
  `ListenTo<T, TConverter>(...)` / `PublishMessage<T, TConverter>(...)`.
- **New test:** add an xUnit `[Fact]` method; follow the existing per-message-file convention so
  the suite stays navigable.

## Dependencies

- `Common.Logging` 2.1.2 — logging facade used throughout (`LogManager.GetCurrentClassLogger()`).
- Rx 2.1.30214.0 — `System.Reactive.*`; core to the subscription model.
- `xunit` 1.9.2 (test runner) and `Moq` 4.1.1309.0801 (test mocks).
