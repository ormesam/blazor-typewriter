# Blazor Typewriter

## Installation

To Install

```
Install-Package BlazorTypewriter
```

or

```
dotnet add package BlazorTypewriter
```

Add stylesheet to head section

```
<link href="_content/BlazorTypewriter/styles.css" rel="stylesheet" />
```

## Usage

![demo](data/demo.gif)

```html
<p><Typewriter Builder="@typewriter" /></p>

@code {
    TypewriterBuilder typewriter = new TypewriterBuilder(defaultCharacterPause: 6)
        .TypeString("First line... Lorem ipsum dolor sit amet, consectetur adipiscing elit.")
        .Pause(1000)
        .DeleteAll()
        .TypeString("Second line... Lorem ipsum dolor sit amet, consectetur adipiscing elit.", 50)
        .Pause(500)
        .DeleteAll(30)
        .TypeString("Third line... Lorem ipsum dolor sit amet, consectetur adipiscing elit.", 20)
        .Pause(500)
        .DeleteAll(20)
        .Pause(500)
        .Loop();
}
```

To customise the cursor set the `CustomClass` property of the `<Typewriter>` tag and target the `border-color`.

## License

This project is licensed under the terms of the [MIT license](https://github.com/ormesam/blazor-typewriter/blob/master/LICENSE).
