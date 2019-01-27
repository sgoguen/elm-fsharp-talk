# Elm and F# - Part 1

-----------------------------------------------------------

## Introductions

* Q: Vince, who are you?
* Q: Steve, who are you?

-----------------------------------------------------------

## Why Elm?

* Q: Vince, why should someone look at Elm?  What was the vision for Elm?

-----------------------------------------------------------

## Why F#?

* Q: Steve, why should someone look at F#?   What was the vision for F#?

    F# began in the early 2000s in the mind of Don Syme, who was responsible for adding generics to the .NET runtime.
    The .NET Common Language Runtime was designed to act as a multilanguage runtime and Don wanted to port OCaml to .NET
    The result is you get a powerful multiparadigm language that benefits from a lot of time tested ideas from ML an OCaml ported to the .NET runtime.

    Because F# has uniquely powerful features great for implemententing languages and transpilers, there have been no less than half-a-dozen F# to JavaScript translators.  The first was implemented in the mid-2000's, the next was a commercial product called WebSharper that turned into an open source project.  There were a few others, but tonight we're going to focus on Fable, which is the latest and greatest.

    Unlike previous F# to JavaScript translators, Fable opted to leverage the Babel compiler so as to create clean output using well-tested components.  With that, Fable allows F# developers to use the existing NPM libraries.

-----------------------------------------------------------

## Getting Started with Elm

* Q: Vince, how do you get up and running with an Elm project?
    * A: Create a project with 'elm init' and run with with 'elm reactor'
    * Q: Vince, What does elm init create?

        The idea is that Elm projects should be so simple that nobody needs a tool to generate a bunch of stuff. This also captures the fact that project structure should evolve organically as your application develops, never ending up exactly the same as other projects. - Evan Czaplicki (creator of Elm)

    * Play around with 'elm reactor'

* Q: Vince, does elm have a package manager?

    Hint: In JavaScript folks run npm install to start projects. "Gotta download everything!" But why download packages again and again? Instead, Elm caches packages in /Users/vincecampanale/.elm so each one is downloaded and built ONCE on your machine. Elm projects check that cache before trying the internet. This reduces build times, reduces server costs, and makes it easier to work offline. 

* Q: Vince, how do you build deployable artifacts?

    As a result elm install is only for adding dependencies to elm.json, whereas elm make is in charge of gathering dependencies and building everything. So maybe try elm make instead? - elm install command (also Evan)    

-----------------------------------------------------------

## Getting Started with F#

* Q: Steve, how do you get up and running with an F# project?

    I have to give credit to Elm for providing *one path* and making getting up and running with a minimal number of files.  The F# experience is a little more involved, but if pressed, I would recommend the SAFE Stack.

    1. Install the latest .NET Core
    2. Make sure FAKE (F# Make) is installed: `dotnet tool install fake-cli -g`
    3. Install the SAFE Stack project template with `dotnet new -i SAFE`    
    4. Create the project with `dotnet new SAFE`

    

    

-----------------------------------------------------------