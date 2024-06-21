public class Spell{
    public string? Name{get;set;}
    public string? Category{get;set;}
    public string? Rarity{get;set;}
    public string? Id{get;set;}
    public string[]? Trait{get;set;}
    public List<string>? Source{get;set;}
    public int? Level{get;set;}

    public int[]? Area{get;set;}
    public string? Area_raw{get;set;}
    public int? Range{get;set;}
    public string? Range_raw{get;set;}
    public string? Spell_type{get;set;}
    public string? Type{get;set;}
    public string? Summary{get;set;}
    public string[]? Heighten_group{get;set;}
    public int[]? Heighten_level{get;set;}
    public string? Markdown{get;set;}
    public string? Text{get;set;}
    public string[]? Tradition{get;set;}
    public string? Actions{get;set;}
    public int? Actions_number{get;set;}
    public string? Duration_raw{get;set;}
    public string? Saving_throw{get;set;}
    public string? Target{get;set;}
}