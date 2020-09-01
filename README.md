# Blazor Typewriter

## Installation

// TODO: nuget package

Add stylesheet to head section

```
<link href="_content/BlazorTypewriter/styles.css" rel="stylesheet" />
```

## Usage

![demo](data\demo.gif)

```html
<p><Typewriter Factory="@typewriter" /></p>

@code {
    TypewriterFactory typewriter = new TypewriterFactory(defaultCharacterPause: 6)
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

## License

This project is licensed under the terms of the [MIT license](https://github.com/ormesam/blazor-typewriter/blob/master/LICENSE).