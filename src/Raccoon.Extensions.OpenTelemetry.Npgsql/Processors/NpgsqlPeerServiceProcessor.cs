using System.Diagnostics;
using OpenTelemetry;

namespace Raccoon.Extensions.OpenTelemetry.Npgsql.Processors;

/// <summary>
/// Enriches Npgsql activity spans with <c>peer.service</c> and <c>db.system</c> attributes
/// to enable proper service graph visualization in observability backends like Grafana Tempo.
/// </summary>
/// <remarks>
/// <para>
/// Npgsql 10.x uses the newer OTel semantic convention <c>db.system.name</c>, but many
/// observability backends (including Grafana Tempo) still rely on the legacy <c>db.system</c>
/// attribute in their default <c>virtual_node_peer_attributes</c> when building service graphs.
/// </para>
/// <para>
/// This processor bridges the gap by adding <c>peer.service</c> (highest priority in Tempo's
/// default <c>peer_attributes</c>) and <c>db.system</c> (legacy convention) to every Npgsql span,
/// ensuring PostgreSQL appears as a distinct node in service graphs without requiring backend
/// configuration changes.
/// </para>
/// </remarks>
internal sealed class NpgsqlPeerServiceProcessor : BaseProcessor<Activity>
{
    private const string NpgsqlSourceName = "Npgsql";
    private const string PeerServiceKey = "peer.service";
    private const string PeerServiceValue = "postgresql";
    private const string DbSystemKey = "db.system";
    private const string DbSystemValue = "postgresql";

    /// <summary>
    /// Adds <c>peer.service</c> and <c>db.system</c> tags to Npgsql spans when they complete.
    /// </summary>
    /// <param name="data">The <see cref="Activity"/> being processed.</param>
    public override void OnEnd(Activity data)
    {
        if (data.Source.Name != NpgsqlSourceName)
        {
            return;
        }

        data.SetTag(PeerServiceKey, PeerServiceValue);
        data.SetTag(DbSystemKey, DbSystemValue);
    }
}