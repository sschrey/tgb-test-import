using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;

namespace RemoteServices.Client
{
    public static class WCFExtensions
    {
        /// <summary>        
        /// Safely closes a service client connection.        
        /// </summary>        
        /// <param name="myServiceClient">The client connection to close.</param>        
        public static void CloseConnection(this ICommunicationObject myServiceClient)
        {
            if (myServiceClient.State != CommunicationState.Opened)
            {
                return;
            } 
            try
            {
                myServiceClient.Close();
            }
            catch (CommunicationException ex)
            {
                Debug.Print(ex.ToString());
                myServiceClient.Abort();
            }
            catch (TimeoutException ex)
            {
                Debug.Print(ex.ToString());
                myServiceClient.Abort();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
                myServiceClient.Abort();
                throw;
            }
        }
    }
}
