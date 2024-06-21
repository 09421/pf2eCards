// See https://aka.ms/new-console-template for more information

using pf2eTemplates;

var eSearch = new ESearch();
var result = await eSearch.SearchCantrips();

foreach(var spell in result)
    AddText.CreateSpellCard(spell.Name, spell.Markdown, spell.Trait, spell.Tradition, spell.Actions_number, spell.Actions, spell.Range_raw, spell.Area_raw, spell.Duration_raw, spell.Saving_throw, spell.Target);
