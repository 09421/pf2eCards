using System.Drawing;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using Elastic.Clients.Elasticsearch;
using Markdig;

namespace pf2eTemplates;

[SupportedOSPlatform("windows")]
public class AddText
{
    public static string OneActionIcon = '\uE902'.ToString();
    public static string TwoActionIcon = '\uE901'.ToString();
    public static string ThreeActionIcon = '\uE900'.ToString();
    public static string FreeActionIcon = '\uE903'.ToString();
    public static string ReactionIcon = '\uE904'.ToString();
    
    private static string GetActionIcon(int? actionNumber, string? action) 
    {
        switch(actionNumber)
        {
            case 0: return FreeActionIcon;
            case 2: return OneActionIcon;
            case 4: return TwoActionIcon;
            case 6: return ThreeActionIcon;
            case 8: return ReactionIcon;
            default: return action;
        }
    }
    [SupportedOSPlatform("windows")]
    public static void CreateSpellCard(string Name, string text, string[] traits, string[] traditions, int? actionNumber, string? actions, string range, string area, string duration, string saving_throw, string target)
    {
        // Height: 1050
        // Width: 750
        
        var Starting_x = 20.0f;
        var rectY = 20.0f;
        var template = Image.FromFile(@"template\SpellTemplate.png");

        // Create font and brush.
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        Pen TitlePen = new Pen(Color.Black, 5f);
        Pen SubPen = new Pen(Color.Black, 2f);

        // Create Title rectangle for drawing.
        string Title = Name.ToUpper();
        float Titlex = Starting_x;
        float Titley = rectY;
        float Titlewidth = 710.0F;
        float Titleheight = 40.0F;
        RectangleF titleRect = new RectangleF(Titlex, Titley, Titlewidth, Titleheight);        
        StringFormat TitledrawFormat = new StringFormat();
        TitledrawFormat.Alignment = StringAlignment.Center;
        rectY += Titleheight;

        //add traits
        var traitsString = string.Join(", ", traits);
        float traitsx = Starting_x;
        float traitsy = rectY+4;
        float Ttraitswidth = 710.0F;
        float traitsheight = 30.0F;
        rectY += traitsheight;
        RectangleF traitsRect = new RectangleF(traitsx, traitsy, Ttraitswidth, traitsheight);
        StringFormat traitsdrawFormat = new StringFormat();
        traitsdrawFormat.Alignment = StringAlignment.Center;

        //add Traditions
        string traditionsString = null;
        RectangleF TraditionsRect = new RectangleF();
        if(traditions is not null)
        {
            traditionsString = string.Join(", ", traditions);
            float Traditionsx = Starting_x;
            float Traditionsy = rectY+4;
            float Traditionswidth = 710.0F;
            float Traditionsheight = 40.0F;
            rectY += Traditionsheight;
            TraditionsRect = new RectangleF(Traditionsx, Traditionsy, Traditionswidth, Traditionsheight);
            StringFormat TraditionsdrawFormat = new StringFormat();
            TraditionsdrawFormat.Alignment = StringAlignment.Near; 
        }       

        //Add Action
        var ActionIcon = GetActionIcon(actionNumber, actions);
        float Actionx = Starting_x;
        float Actiony = rectY+4;
        float Actionwidth = 710.0F;
        float Actionheight = 40.0F;
        rectY += Actionheight;
        RectangleF ActionRect = new RectangleF(Actionx, Actiony, Actionwidth, Actionheight);
        StringFormat ActiondrawFormat = new StringFormat();
        ActiondrawFormat.Alignment = StringAlignment.Near;

        //Add range
        string RangeText = null; 
        RectangleF range_Rect = new RectangleF();
        if(range is not null || area is not null || target != null)
        {
            RangeText = range != null ? range : null;
            RangeText += (range != null && (area != null || target != null)) ? ";" : "";
            float range_x = Starting_x;
            float range_y = rectY+4;
            float range_width = 710.0F;
            float range_height = 40.0F;
            rectY += range_height;
            range_Rect = new RectangleF(range_x, range_y, range_width, range_height);
            StringFormat range_drawFormat = new StringFormat();
            range_drawFormat.Alignment = StringAlignment.Near;
        }
    
        RectangleF Saving_Rect = new RectangleF();
        string defenseText = null;
        string durationText = null;
        if(saving_throw is not null || duration is not null)
        {
            durationText = duration;
            if(saving_throw is not null){
                defenseText = char.ToUpper(saving_throw[0]) + saving_throw.Substring(1).Replace("  ", " ");            
                defenseText += (saving_throw != null && duration != null) ? ";" : "";
            }
            float Saving_x = Starting_x;
            float Saving_y = rectY+4;
            float Saving_width = 710.0F;
            float Saving_height = 40.0F;
            rectY += Saving_height;
            Saving_Rect = new RectangleF(Saving_x, Saving_y, Saving_width, Saving_height);
            StringFormat range_drawFormat = new StringFormat();
            range_drawFormat.Alignment = StringAlignment.Near;
        }


        // Create Text rectangle for drawing.
        var rulesText = text.Split("---")[1];
        float Textx = Starting_x;
        float Texty = rectY+6;
        float Textwidth = 710.0F;
        float Textheight = 0;
        RectangleF textRect = new RectangleF(Textx, Texty, Textwidth, Textheight);
        StringFormat TextdrawFormat = new StringFormat();
        TextdrawFormat.Alignment = StringAlignment.Near;


        var Crit_Pattern = @"\*\*(.*?)\*\*";
        Regex crit_rgx = new Regex(Crit_Pattern, RegexOptions.IgnoreCase);            
        var TextAndCritSplitList = crit_rgx.Split(rulesText).ToList();
        rulesText = Markdown.ToPlainText(TextAndCritSplitList[0]).Replace("<ul><li>", "");


        // Create Heightend rectangle for drawing.
        var HeightenedList = new List<string>();
        if(text.Split("---").Count() == 3)
        {
            var Heightend_text = text.Split("---")[2];

            var Heightend_Pattern = @"\*\*(.*?)\*\*";

            Regex rgx = new Regex(Heightend_Pattern, RegexOptions.IgnoreCase);            
            HeightenedList = rgx.Split(Heightend_text).ToList();
            HeightenedList.RemoveAt(0);
        }

        //TODO: reaction and cast time


        using(Graphics g = Graphics.FromImage(template))
        {
            using(Font arialFont = new("Arial", 6, FontStyle.Regular))
            using(Font arialFontBOLD = new("Arial", 6, FontStyle.Bold))
            using(Font arialFontSmall = new("Arial", 5, FontStyle.Italic))
            using(Font PathfinderFont = new("Pathfinder-Icons", 7))
            {
                //Title
                g.DrawString(Title, arialFontBOLD, drawBrush, titleRect, TitledrawFormat);
                g.DrawLine(TitlePen, titleRect.X, titleRect.Y+titleRect.Height, titleRect.X+titleRect.Width, titleRect.Y+titleRect.Height);

                //Traits
                g.DrawString(traitsString, arialFontSmall, drawBrush, traitsRect, traitsdrawFormat);
                g.DrawLine(SubPen, traitsRect.X, traitsRect.Y+traitsRect.Height, traitsRect.X+traitsRect.Width, traitsRect.Y+traitsRect.Height);
                
                //Traditions
                if(traditionsString is not null)
                {
                    g.DrawString("Traditions", arialFontBOLD, drawBrush, TraditionsRect, TextdrawFormat);                
                    TraditionsRect.X += g.MeasureString("Traditions", arialFontBOLD).Width;
                    g.DrawString(traditionsString, arialFont, drawBrush, TraditionsRect, TextdrawFormat);
                }
                
                // Actions
                g.DrawString("Cast", arialFontBOLD, drawBrush, ActionRect);
                ActionRect.X += g.MeasureString("Cast", arialFontBOLD).Width;
                ActionRect.Y += actionNumber > 10 ? 0 : 3;
                g.DrawString(ActionIcon, actionNumber > 10 ? arialFont : PathfinderFont, drawBrush, ActionRect);
                
                //Range and Area
                if(!String.IsNullOrEmpty(RangeText))
                {
                    g.DrawString("Range", arialFontBOLD, drawBrush, range_Rect, TextdrawFormat);
                    range_Rect.X += g.MeasureString("Range", arialFontBOLD).Width;
                    g.DrawString(RangeText, arialFont, drawBrush, range_Rect, TextdrawFormat);
                    range_Rect.X += g.MeasureString(RangeText, arialFont).Width;
                }
                //Area
                if(area is not null)
                {
                    g.DrawString("Area", arialFontBOLD, drawBrush, range_Rect, TextdrawFormat);
                    range_Rect.X += g.MeasureString("Area", arialFontBOLD).Width;
                    g.DrawString(area, arialFont, drawBrush, range_Rect, TextdrawFormat);
                }
                //Target
                if(target is not null)
                {
                    g.DrawString("Targets", arialFontBOLD, drawBrush, range_Rect, TextdrawFormat);
                    range_Rect.X += g.MeasureString("Targets", arialFontBOLD).Width;
                    

                    range_Rect.Width -= range_Rect.X+Starting_x;
                    SizeF target_Size = g.MeasureString(target, arialFont, textRect.Size);

                    if(target_Size.Width + range_Rect.X >= 710.0F)
                    {
                        range_Rect.Height += 20;
                        g.DrawString(target, arialFont, drawBrush, range_Rect);
                    }
                    else
                    {                        
                        g.DrawString(target, arialFont, drawBrush, range_Rect, TextdrawFormat);
                    }
                }
                //Defense and duration
                if(defenseText is not null)
                {
                    g.DrawString("Defense", arialFontBOLD, drawBrush, Saving_Rect, TextdrawFormat);
                    Saving_Rect.X += g.MeasureString("Defense", arialFontBOLD).Width;
                    g.DrawString(defenseText, arialFont, drawBrush, Saving_Rect, TextdrawFormat);
                    Saving_Rect.X += g.MeasureString(defenseText, arialFont).Width;
                }
                //Duration
                if(durationText is not null)
                {
                    g.DrawString("Duration", arialFontBOLD, drawBrush, Saving_Rect, TextdrawFormat);
                    Saving_Rect.X += g.MeasureString("Duration", arialFontBOLD).Width;
                    g.DrawString(durationText, arialFont, drawBrush, Saving_Rect, TextdrawFormat);
                }

                //Rules text 
                g.DrawLine(SubPen, textRect.X, textRect.Y-2, textRect.X+textRect.Width, textRect.Y-2);
                SizeF size = g.MeasureString(rulesText, arialFont, textRect.Size);
                g.DrawString(rulesText, arialFont, drawBrush, textRect, TextdrawFormat);

                //Crit Info
                if(TextAndCritSplitList.Count() > 1)
                {
                    textRect.Y += size.Height+5;
                    for(var i = 1; i < TextAndCritSplitList.Count(); i+=2)
                    {
                        g.DrawString(Markdown.ToPlainText(TextAndCritSplitList[i]), arialFontBOLD, drawBrush, textRect, TextdrawFormat);
                        textRect.Y += 35;
                        g.DrawString(Markdown.ToPlainText(TextAndCritSplitList[i+1]).Trim(), arialFont, drawBrush, textRect, TextdrawFormat);
                        SizeF HeightenedTextsize = g.MeasureString(TextAndCritSplitList[i+1].Trim(), arialFont, textRect.Size);
                        textRect.Y += HeightenedTextsize.Height+5;
                    }
                    textRect.Y += 5;
                }
                else{                    
                    textRect.Y += size.Height+5;
                }
                
                //Heightened text
                if(HeightenedList.Count() > 0)
                {
                    g.DrawLine(SubPen, textRect.X, textRect.Y+textRect.Height, textRect.X+textRect.Width, textRect.Y+textRect.Height);
                    textRect.Y += 5;
                    for(var i = 0; i < HeightenedList.Count(); i+=2)
                    {
                        g.DrawString(HeightenedList[i], arialFontBOLD, drawBrush, textRect, TextdrawFormat);
                        textRect.Y += 35;
                        g.DrawString(HeightenedList[i+1].Trim(), arialFont, drawBrush, textRect, TextdrawFormat);
                        SizeF HeightenedTextsize = g.MeasureString(HeightenedList[i+1].Trim(), arialFont, textRect.Size);
                        textRect.Y += HeightenedTextsize.Height+5;
                    }
                }
            }
        }


        template.Save($@"Cards\Spells\{Name}.png");
        TitlePen.Dispose();
        SubPen.Dispose();
    }
}
