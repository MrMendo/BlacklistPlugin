using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Party;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.UI.Info;

namespace BlacklistPlugin
{
    public class PartyListUpdater
    {
        private PartyList partyList { get; }
        private bool enabled;
        private int previousPartySize;
        public PartyListUpdater(PartyList partyList) {
            this.partyList = partyList;
            this.enabled = false;
            this.previousPartySize = getPartySize();
        }
        
        private async void StartAsyncUpdate()
        {
            while (this.enabled)
            {
                await Task.Delay(500);

                PluginLog.Verbose("Henlo this an update");
                if (isPartySizeChanged())
                {
                    PluginLog.Verbose($"Current PartySize changed from: {this.previousPartySize} to {getPartySize()}");
                    doStuff();
                }

                this.previousPartySize = getPartySize();
            }
        }

        private unsafe void doStuff()
        {
            if (InfoProxyCrossRealm.IsCrossRealmParty())
            {
                InfoProxyCrossRealm.GetGroupMember(1);
            } else
            {
                // TODO: Implement for non xRealm Parties
            }
        }

        public int getPartySize()
        {
            int memberCount;
            if (InfoProxyCrossRealm.IsCrossRealmParty())
            {
                memberCount = InfoProxyCrossRealm.GetPartyMemberCount();
            } else
            {
                memberCount = this.partyList.Length;
            }

            return Math.Max(memberCount, 1);
        }

        private bool isPartySizeChanged()
        {
            return this.previousPartySize != getPartySize();
        }

        public void Enable()
        {
            this.enabled = true;
            StartAsyncUpdate();
        }

        public void Disable()
        {
            this.enabled = false;
        }

        public void Dispose() 
        {
            Disable();    
        }
    }
}
