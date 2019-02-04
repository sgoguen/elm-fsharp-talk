module App.Update

open App.Model
open Elmish

//-------------------------------------

// UPDATE


(** Users of our app can trigger messages by clicking
 and typing. These messages are fed into the `update`
 function as they occur, letting us react to them.
*)
type Msg =
    | Failure of string
    | UpdateField of string
    | EditingEntry of int*bool
    | UpdateEntry of int*string
    | Add
    | Delete of int
    | DeleteComplete
    | Check of int * bool
    | CheckAll of bool
    | ChangeVisibility of string

// How we update our Model on a given Msg?
let update (msg:Msg) (model:Model) : Model =
    match msg with
    | Failure err ->
        Fable.Import.Browser.console.error(err)
        model

    | Add ->
        { model with
            uid = model.uid + 1
            field = ""
            entries = 
                if System.String.IsNullOrEmpty model.field then
                    model.entries
                else

                    model.entries @ [newEntry model.field model.uid]            
        }
    
    | UpdateField str ->
        { model with field = str }
    
    | EditingEntry (id,isEditing) ->
        let updateEntry t =
          if t.id = id then 
            { t with editing = isEditing } 
          else 
            t
        { model with 
            entries = 
              List.map updateEntry model.entries }
    
    | UpdateEntry (id,task) ->
        let updateEntry t =
          if t.id = id then { t with description = task } else t
        { model with entries = List.map updateEntry model.entries }

    | Delete id ->
        { model with entries = List.filter (fun t -> t.id <> id) model.entries }
    
    | DeleteComplete ->
        { model with entries = List.filter (fun t -> not t.completed) model.entries }
    
    | Check (id,isCompleted) ->
        let updateEntry t =
          if t.id = id then { t with completed = isCompleted } else t
        { model with entries = List.map updateEntry model.entries }

    | CheckAll isCompleted ->
        let updateEntry t = { t with completed = isCompleted }
        { model with entries = List.map updateEntry model.entries }

    | ChangeVisibility visibility ->
        { model with visibility = visibility }
