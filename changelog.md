# 1.2.0
## Fixed
- **Raccoon.Extensions.OpenTelemetry.Npgsql**: PostgreSQL now appears as a distinct node in Grafana Tempo service graphs. Npgsql 10.x uses the newer `db.system.name` semantic convention, but Tempo's default `virtual_node_peer_attributes` expects the legacy `db.system` attribute. Added a span processor that enriches Npgsql spans with `peer.service` and `db.system` attributes, ensuring service graph compatibility without requiring backend configuration changes.

## Updated
- Updated general dependencies to latest versions.

No other code changes for the other packages.

# 1.1.0
## Added
- New convenience `IConfigurationManager` extension method to get the Open Telemetry Endpoint called `GetOpenTelemetryEndpoint` to help fetch the Open Telemetry Endpoint from the configuration/appsettings.

# 1.0.1
## Organization
No code changes, just organizing files, and including readme and license in the packages.

# 1.0.0
## Initial Release
- Initial version of the library