using System;
using System.Collections;
using UnityEngine.Networking;

namespace Assets.Scripts.Networking
{
    public static class RandomUtility
    {
        private const string Url = "https://www.random.org/integers/?num=1&min=1&max=6&col=1&base=10&format=plain&rnd=new";

        public static IEnumerator FetchDiceRandomNumber(Action<int> success, Action<string> fail)
        {
            // Create a UnityWebRequest object.
            var request = UnityWebRequest.Get(Url);

            // Start the request.
            request.SendWebRequest();

            // Check if the request is complete.
            while (!request.isDone)
            {
                // Yield control to the next frame.
                yield return null;
            }

            try
            {
                var result = Convert.ToInt32(request.downloadHandler.text);

                success(result);
            }
            catch (Exception ex)
            {
                fail("Error: " + request.error + " " + ex.Message);
            }

            // Close the request.
            request.Dispose();
        }
    }
}
