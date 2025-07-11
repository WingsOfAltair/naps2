using Eto.Drawing;
using Eto.Mac.Drawing;

namespace NAPS2.EtoForms.Mac;

public class MacIconProvider : IIconProvider
{
    private static readonly Dictionary<string, string> IconMap = new()
    {
        { "control_play_blue", "play" },
        { "add", "plus.circle" },
        { "pencil", "pencil" },
        { "cross", "trash" },
        { "cross_small", "xmark" },
        { "accept", "checkmark.circle" },
        { "wireless", "wifi" },
        { "blueprints", "list.bullet" },
        { "folder_picture", "folder" },
        { "diskette", "square.and.arrow.down" },
        { "zoom", "viewfinder" },
        { "arrow_rotate_anticlockwise", "arrow.counterclockwise" },
        { "arrow_rotate_clockwise", "arrow.clockwise" },
        { "arrow_switch", "arrow.2.squarepath" },
        { "arrow_up", "arrow.up" },
        { "arrow_down", "arrow.down" },
        { "arrow_left", "arrow.left" },
        { "arrow_right", "arrow.right" },
        { "transform_crop", "crop" },
        { "weather_sun", "sun.max" },
        { "contrast_with_sun", "sun.max" },
        { "color_management", "paintpalette" },
        { "color_wheel", "paintpalette" },
        { "color_gradient", "square.righthalf.filled" },
        { "contrast", "circle.righthalf.filled" },
        { "contrast_high", "circle.righthalf.filled" },
        { "sharpen", "rhombus" },
        { "file_extension_pdf", "doc.richtext" },
        { "pictures", "photo" },
        { "document", "doc.text" },
        { "split", "squareshape.split.2x2.dotted" },
        { "text_align_justify", "text.justify" },
        { "large_tiles", "square.grid.2x2" },
        { "exclamation", "exclamationmark.triangle" },
        { "application_side_list", "sidebar.left" },
        { "ask", "questionmark" },
        { "split_ver", "square.split.2x1" },
        { "split_hor", "square.split.1x2" },
        { "shape_align_left", "align.horizontal.left" },
        { "shape_align_center", "align.horizontal.center" },
        { "shape_align_right", "align.horizontal.right" },
        { "shape_align_top", "align.vertical.top" },
        { "shape_align_middle", "align.vertical.center" },
        { "shape_align_bottom", "align.vertical.bottom" },
        { "combine", "square.split.1x2" },
        { "combine_hor", "square.split.2x1" },
        { "combine_ver", "square.split.1x2" },
        { "switch_hor", "arrow.left.and.right" },
        { "switch_ver", "arrow.up.and.down" }
        // TODO: This doesn't render properly as it's very wide and gets squished
        // { "keyboard", "keyboard" },
        // TODO: Consider these
        // { "network_ip", "wifi.router" },
        // { "device", "scanner" },
    };

    private readonly DefaultIconProvider _defaultIconProvider;

    public MacIconProvider(DefaultIconProvider defaultIconProvider)
    {
        _defaultIconProvider = defaultIconProvider;
    }

    public Bitmap? GetIcon(string name, float scale = 1f, bool oversized = false)
    {
        if (!OperatingSystem.IsMacOSVersionAtLeast(11) && name == "arrow_rotate_anticlockwise")
        {
            // TODO: Verify this fixes the rotate menu on macOS 10.15
            // TODO: Also maybe map other icons to 16x16 versions (e.g. control_play_blue) for better rendering
            return _defaultIconProvider.GetIcon("arrow_rotate_anticlockwise_small");
        }
        if (OperatingSystem.IsMacOSVersionAtLeast(11))
        {
            if (!IconMap.ContainsKey(name) && name.EndsWith("_small"))
            {
                name = name.Substring(0, name.Length - 6);
            }
            if (IconMap.ContainsKey(name))
            {
                var symbol = NSImage.GetSystemSymbol(IconMap[name], null);
                if (symbol != null)
                {
                    if (oversized)
                    {
                        symbol = symbol.GetImage(NSImageSymbolConfiguration.Create(32, 0.1))!;
                    }
                    if (name == "ask")
                    {
                        // Needs to be rendered at fixed dimensions to display properly in the Choose Device listview
                        symbol = symbol.GetImage(NSImageSymbolConfiguration.Create(60, 0.1));
                        return (Bitmap) new Bitmap(new BitmapHandler(symbol)).PadTo(new Size(64, 64));
                    }
                    return new Bitmap(new BitmapHandler(symbol));
                }
            }
        }
        return _defaultIconProvider.GetIcon(name, scale);
    }

    public Icon? GetFormIcon(string name, float scale = 1f) => null;
}