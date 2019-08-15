using IpStack;
using IpStack.Models;

namespace ThirtyMinutes.Helpers
{
    public static class IpAddressHelper
    {
        public static IpAddressDetails GetIpAddressDetails(string ipAddress)
        {
            var client = new IpStackClient("f960c404b071baa814ddb8092616afa3");            
            return client.GetIpAddressDetails(ipAddress);            
        }
    }
}