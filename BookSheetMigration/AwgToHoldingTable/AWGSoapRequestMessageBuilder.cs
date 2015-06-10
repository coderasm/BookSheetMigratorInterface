using System.Collections.Generic;

namespace BookSheetMigration
{
    public class AWGSoapRequestMessageBuilder : SoapRequestMessageBuilder
    {

        public AWGSoapRequestMessageBuilder(string action, Dictionary<string, string> actionArguments) : base(action, actionArguments)
        {
        }

        protected override void addCredentialsToAction(SoapAction soapAction)
        {
            soapAction.addParameterPairToAction("securityToken", Settings.securityToken);
            soapAction.addParameterPairToAction("clientUsername", Settings.clientUsername);
            soapAction.addParameterPairToAction("clientPassword", Settings.password);
        }
    }
}
