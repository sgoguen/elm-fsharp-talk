(**
 - title: Todo MVC
 - tagline: The famous todo mvc ported from elm-todomvc
*)

module App.Main

open Elmish

open App.Model
open App.Storage
open App.Views

open Elmish.React
//-------------------------------------

//  We start our program here because F# tends to act
//  act like a one-pass compiler.

open Elmish.Debug
// App
Program.mkProgram (S.load >> init) updateWithStorage view
|> Program.withReact "todoapp"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
