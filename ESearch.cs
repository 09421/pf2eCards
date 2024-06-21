using Elastic.Clients.Elasticsearch;

public class ESearch
{
    public ElasticsearchClient Client{get;set;}
    public string root = "https://elasticsearch.aonprd.com/";
    public string index = "aon";
    public ESearch()
    {
        Client = new ElasticsearchClient(new Uri(root));
    }

    public async Task<List<Spell>> SearchCantrips()
    {
        var result = await Client.SearchAsync<Spell>(s => s
            .Index(index)
            .From(0)
            .Size(50)
            .Query(q => q
                .Bool(b => b
                    .Must(mu => mu
                        .MatchPhrase(m => m 
                            .Field(f => f.Category)
                            .Query("spell")
                        ), mu => mu
                        .Match(m => m 
                            .Field(f => f.Spell_type)
                            .Query("Cantrip")
                        ), mu => mu
                        .Match(m => m 
                            .Field(f => f.Name)
                            .Query("Slashing Gust")
                        )                      
                    )
                )
            )
        );

        if(result.IsValidResponse){
            var t = result.Documents.ToList();
            return t;
        }

        return [];
    }
}