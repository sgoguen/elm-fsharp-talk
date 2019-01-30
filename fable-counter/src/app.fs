(**
 - title: Counter
*)

module App

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Elmish
open Elmish.React
open Elmish.Debug


//  *** MODEL ***
type Model = int

//  *** UPDATE *** 
type Msg = 
    | Increment
    | Decrement

//  Define an update function
let update msg model = 
    match msg with
    | Increment -> (model + 1, [])
    | Decrement -> (model - 1, [])

//  *** VIEW ***
let view (model:Model) dispatch =
    div [] [ 
        button [ OnClick(fun _ -> dispatch(Increment)) ] [ Text "+" ]
        div [] [ Text (string(model)) ]
        button [ OnClick(fun _ -> dispatch(Decrement)) ] [ Text "-" ]
        ]



//-------------------------------------
//  We start our program here because F# tends to act
//  act like a one-pass compiler.

// App
let init _ = (0, [])

Program.mkProgram init update view
    |> Program.withReact "counterapp"
    #if DEBUG
    |> Program.withDebugger
    #endif
    |> Program.run
