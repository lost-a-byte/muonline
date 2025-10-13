
using Client.Main.Models;
using Microsoft.Xna.Framework;

namespace Client.Main.Controls.UI;

public class SelectOptionControl : UIControl
{
    public KeyValuePair<string, int> Value
    {
        get => optionPicker.Value;
        set => optionPicker.Value = value;
    }

    public string Text
    {
        get => Button.Label.Text;
        set => Button.Label.Text = value;
    }

    public event EventHandler<KeyValuePair<string, int>> ValueChanged;

    public List<KeyValuePair<string, int>> Options
    {
        get => optionPicker.Options;
        set
        {
            optionPicker.Options = value;
        }
    }

    private ControlAlign buttonAlign = ControlAlign.Top;
    public ControlAlign ButtonAlign
    {
        get => buttonAlign;
        set => buttonAlign = value;
    }
    private SpriteControl Icon { get; }
    private LabelButton Button { get; }

    private readonly OptionPickerControl optionPicker = new()
    {
        Visible = false,
    };

    public SelectOptionControl()
    {
        Interactive = true;
        Button = new LabelButton
        {
            Label = new LabelControl
            {
                X = 8,
                Align = ControlAlign.VerticalCenter,
            },
            Align = this.Align,
            X = 9,
        };

        AutoViewSize = true;
        Icon = new SpriteControl
        {
            X = 180 - 16 - 8,
            Y = 6,
            Interactive = false,
            TileWidth = 32,
            TileHeight = 32,
            Scale = 0.5f,
            ViewSize = new Point(32, 32),
            TileY = 0,
            BlendState = Blendings.Alpha,
            TexturePath = "Interface/GFx/chat_btsize02.dds"
        };
    }

    public override async Task Initialize()
    {
        Controls.Add(Button);
        Button.Align = Align;
        Button.Controls.Add(Icon);
        Button.Click += OnButtonClick;
        Controls.Add(optionPicker);
        optionPicker.Options = Options;
        optionPicker.ValueChanged += OnValueChanged;
        if (ButtonAlign == ControlAlign.Bottom)
        {
            optionPicker.Margin = new Margin
            {
                Bottom = Button.ViewSize.Y,
            };
        }
        else
        if (ButtonAlign == ControlAlign.Top)
        {
            optionPicker.Margin = new Margin
            {
                Top = Button.ViewSize.Y,
            };
        }
        await base.Initialize();
    }

    public override void Dispose()
    {
        Button.Click -= OnButtonClick;
        optionPicker.ValueChanged -= OnValueChanged;
        Icon.Dispose();
        optionPicker.Dispose();
        base.Dispose();
    }

    public override bool ProcessMouseScroll(int scrollDelta)
    {
        return optionPicker.ProcessMouseScroll(scrollDelta);
    }

    private void OnButtonClick(object sender, EventArgs e)
    {
        optionPicker.Visible = !optionPicker.Visible;
    }
    private void OnValueChanged(object sender, KeyValuePair<string, int> e)
    {
        Text = e.Key;
        ValueChanged?.Invoke(this, e);
    }
}