using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Skia;
using SkiaSharp;

namespace Toradex;

public class CustomFontManagerImpl : IFontManagerImpl
{
    private readonly string[] _bcp47 =
        { CultureInfo.CurrentCulture.ThreeLetterISOLanguageName, CultureInfo.CurrentCulture.TwoLetterISOLanguageName };

    private readonly Typeface[] _customTypefaces;
    private readonly string _defaultFamilyName;

    private readonly Typeface _defaultTypeface =
        new("resm:Toradex.Assets?assembly=Toradex#Proxima Nova");

    public CustomFontManagerImpl()
    {
        _customTypefaces = new[] { _defaultTypeface };
        _defaultFamilyName = _defaultTypeface.FontFamily.FamilyNames.PrimaryFamilyName;
    }

    public string GetDefaultFontFamilyName()
    {
        return _defaultFamilyName;
    }

    public IEnumerable<string> GetInstalledFontFamilyNames(bool checkForUpdates = false)
    {
        return _customTypefaces.Select(x => x.FontFamily.Name);
    }

    public bool TryMatchCharacter(int codepoint, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch,
        FontFamily? fontFamily, CultureInfo? culture, out Typeface typeface)
    {
        var res = TryMatchCharacter(codepoint, fontStyle, fontWeight, fontFamily, culture, out Typeface typeface2);
        typeface = typeface2;
        return res;
    }

    public bool TryMatchCharacter(int codepoint, FontStyle fontStyle, FontWeight fontWeight, FontFamily fontFamily,
        CultureInfo culture, out Typeface typeface)
    {
        foreach (var customTypeface in _customTypefaces)
        {
            if (customTypeface.GlyphTypeface.GetGlyph((uint)codepoint) == 0) continue;

            typeface = new Typeface(customTypeface.FontFamily, fontStyle, fontWeight);

            return true;
        }

        var fallback = SKFontManager.Default.MatchCharacter(fontFamily?.Name, (SKFontStyleWeight)fontWeight,
            (SKFontStyleWidth)fontWeight, (SKFontStyleSlant)fontStyle, _bcp47, codepoint);

        typeface = new Typeface(fallback?.FamilyName ?? _defaultFamilyName, fontStyle, fontWeight);

        return true;
    }

    public IGlyphTypefaceImpl CreateGlyphTypeface(Typeface typeface)
    {
        SKTypeface skTypeface;

        switch (typeface.FontFamily.Name)
        {
            case FontFamily.DefaultFontFamilyName:
            case "Proxima Nova":
            {
                var typefaceCollection =
                    SKTypefaceCollectionCache.GetOrAddTypefaceCollection(_defaultTypeface.FontFamily);
                skTypeface = typefaceCollection.Get(_defaultTypeface);
                break;
            }
            default:
            {
                // skTypeface = SKTypeface.FromFamilyName(typeface.FontFamily.Name,
                //     (SKFontStyleWeight)typeface.Weight, SKFontStyleWidth.Normal, (SKFontStyleSlant)typeface.Style);
                // break;

                var typefaceCollection =
                    SKTypefaceCollectionCache.GetOrAddTypefaceCollection(_defaultTypeface.FontFamily);
                skTypeface = typefaceCollection.Get(_defaultTypeface);
                break;
            }
        }

        return new GlyphTypefaceImpl(skTypeface);
    }
}
