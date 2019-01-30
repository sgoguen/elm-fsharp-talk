# Elm & F# - A Tale of Two Languages

## Introductions

* Who is Vince?  Why Elm?
* Who is Steve?  Why F#?
* Who are you?  Why are you here?

## Agenda

* Tonight is part Show and Tell and Q&A with a loose agenda
  * It will mostly be question driven, but we'll try to hit the big parts.

* Part 1 - Getting Started with Elm and F# (10-30 minutes)
* Part 2 - Walking through Todo MVC (10-30 minutes)
* Part 3 - Elm & F# Difference (10-30 minutes)

-------------------------------------------------------------------------------

## Part 1 - Getting Started (15 minutes)

1. Getting up and running with Elm
  * [`elm init`](https://elm-lang.org/0.19.0/init)
  * `elm reactor`
  > The idea is that Elm projects should be so simple that nobody needs a tool to generate a bunch of stuff. This also captures the fact that project structure should evolve organically as your application develops, never ending up exactly the same as other projects. - Evan Czaplicki (creator of Elm)
  * `elm install` vs. `elm make`
	> Hint: In JavaScript folks run `npm install` to start projects. "Gotta download everything!" But why download packages again and again? Instead, Elm caches packages in /Users/vincecampanale/.elm so each one is downloaded and built ONCE on your machine. Elm projects check that cache before trying the internet. This reduces build times, reduces server costs, and makes it easier to work offline.  As a result elm install is only for adding dependencies to elm.json, whereas elm make is in charge of gathering dependencies and building everything. So maybe try elm make instead? - `elm install` command (also Evan)

2. Getting up and running with F#
  1. Getting Started with F# Fable - https://github.com/fable-compiler/fable2-samples/tree/master/minimal
  2. Walkthrough of NPM, webpack and dotnet core toolset
  3. Counter example
  
-------------------------------------------------------------------------------

## Part 2 - TodoMVC (15-20 minutes)

1. Defining the model in Elm and F# - Side-by-side
2. Update Methods
   * Core methods
   * Explaining Commands
3. View Methods

-------------------------------------------------------------------------------

## Part 3 - Differences (15 - 20 minutes)

## Where Elm Shines
  * Simple tooling
  * Laser focused on *zero* runtime errors
  * Exceptionally fast
  * Curated libraries
  * Ports - Bridges the impure world of JavaScript with Elm's functional purity
  * Excellent community!

## Where Elm has yet to shine
  * No backend programming support
  * There isn't a curated library for everything

## Where F# Shines
  * Same F# code can be used on both front-end and backend
  * Powerful, yet controlled language and meta-language features
    * Type providers
    * Active Patterns
    * Quotations (Allowing F# to be translated to JavaScript)
  * Multi-paradigm - 
  * Fable Compiler
    * Does an excellent job compiling to clean, readable, idiomatic JavaScript
    * Can create
    * Excellent JavaScript inter-op support - Can use [<Emit>] attributes to customize compilation.
  * SAFE Stack
    * Provides a lot out-of-the-box
  * Excellent community!

## Where F# can improve
  * A lot of choice on tooling (We have 5-6 F# to JavaScript compilers)
  * Tooling could be slimmer 
  * Many excellent tools, but others could be friendlier to newbs
  * Experience isn't as cultivated
  
  
  
