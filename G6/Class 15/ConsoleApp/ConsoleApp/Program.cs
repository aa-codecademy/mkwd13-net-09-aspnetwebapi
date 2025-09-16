try
{
	using(HttpClient client = new HttpClient())
	{
		HttpResponseMessage responseMessage = client.GetAsync("http://localhost:5292/api/Test/testUser").Result;
		string responseBodyContent = responseMessage.Content.ReadAsStringAsync().Result;

		Console.WriteLine(responseBodyContent);	
	}
}catch(Exception ex)
{
	Console.WriteLine(ex.Message);
}