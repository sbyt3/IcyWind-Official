using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrix.Xml;
using Matrix.Xmpp.Sasl;

namespace Matrix.Sasl.RiotRSO
{
    public class RiotRSOProcessor : ISaslProcessor
    {
        public async Task<XmppXElement> AuthenticateClientAsync(XmppClient xmppClient, CancellationToken cancellationToken)
        {
            var authMessage = new Auth(SaslMechanism.RiotRSO, xmppClient.Password);

            return
                await xmppClient.SendAsync<Success, Failure>(authMessage, cancellationToken);
        }
    }
}
