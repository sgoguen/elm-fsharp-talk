module App.Model

let [<Literal>] ALL_TODOS = "all"
let [<Literal>] ACTIVE_TODOS = "active"
let [<Literal>] COMPLETED_TODOS = "completed"

//-------------------------------------
// MODEL

(* The first difference in F# is types typically
   must be declared before they're used.  
*)
type Model = 
    { entries : Entry list
      field : string
      uid : int
      visibility : string }

//  The and keyword allows you to reorder it (Why?)
and Entry =
    { description : string
      completed : bool
      editing : bool
      id : int }

//-------------------------------------

// Define a single instance of an empty todo list
let emptyModel =
    { entries = []
      visibility = ALL_TODOS
      field = ""
      uid = 0 
    }

(*
F# allows for type inferencing, but the tools
often let you see what types have been inferred.
The comment below to the right is part of the F#
editor for VS Code.  You can also hover over 
variables.
*)

let newEntry desc id =
  { description = desc
    completed = false
    editing = false
    id = id }

let init = function
  | Some(savedModel) -> (savedModel, [])
  | _ -> (emptyModel, [])      