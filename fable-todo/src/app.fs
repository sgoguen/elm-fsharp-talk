﻿

(**
 - title: Todo MVC
 - tagline: The famous todo mvc ported from elm-todomvc
*)





module App

open Fable.Core
open Fable.Import
open Elmish





//  Notice the F# program begins at the bottom

let [<Literal>] ESC_KEY = 27.
let [<Literal>] ENTER_KEY = 13.
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
let update (msg:Msg) (model:Model) : (Model * Cmd<Msg>) =
    match msg with
    | Failure err ->
        Fable.Import.Browser.console.error(err)
        (model, [])

    | Add ->
        ({ model with
            uid = model.uid + 1
            field = ""
            entries = 
                if System.String.IsNullOrEmpty model.field then
                    model.entries
                else

                    model.entries @ [newEntry model.field model.uid]            
        }, [])

    
    
    | UpdateField str ->
        ({ model with field = str }, [])

    
    
    
    | EditingEntry (id,isEditing) ->
        let updateEntry t =
          if t.id = id then 
            { t with editing = isEditing } 
          else 
            t
        ({ model with 
            entries = 
              List.map updateEntry model.entries }, [])

    
    
    
    
    
    
    | UpdateEntry (id,task) ->
        let updateEntry t =
          if t.id = id then { t with description = task } else t
        ({ model with entries = List.map updateEntry model.entries }, [])








    | Delete id ->
        ({ model with entries = List.filter (fun t -> t.id <> id) model.entries }, [])

    
    
    | DeleteComplete ->
        ({ model with entries = List.filter (fun t -> not t.completed) model.entries }, [])

    
    
    | Check (id,isCompleted) ->
        let updateEntry t =
          if t.id = id then { t with completed = isCompleted } else t
        ({ model with entries = List.map updateEntry model.entries }, [])








    | CheckAll isCompleted ->
        let updateEntry t = { t with completed = isCompleted }
        ({ model with entries = List.map updateEntry model.entries }, [])






    | ChangeVisibility visibility ->
        ({ model with visibility = visibility }, [])



//-------------------------------------

// Local storage interface
module S =
    let private STORAGE_KEY = "elmish-react-todomvc"
    let private decoder = Thoth.Json.Decode.Auto.generateDecoder<Model>()
    let load (): Model option =
        Browser.localStorage.getItem(STORAGE_KEY)
        |> unbox
        |> Core.Option.bind (Thoth.Json.Decode.fromString decoder >> function | Ok r -> Some r | _ -> None)

    let save (model: Model) =
        Browser.localStorage.setItem(STORAGE_KEY, Thoth.Json.Encode.Auto.toString(1, model))


let setStorage (model:Model) : Cmd<Msg> =
    Cmd.attemptFunc S.save model (string >> Failure)

let updateWithStorage (msg:Msg) (model:Model) =
  match msg with
  // If the Msg is Failure we know the model hasn't changed
  | Failure _ -> (model, [])
  | _         -> let (newModel, cmds) = update msg model
                 (newModel, Cmd.batch [ setStorage newModel; cmds ])


//-------------------------------------

// rendering views with React
module R = Fable.Helpers.React
open Fable.Core.JsInterop
open Fable.Helpers.React.Props
open Elmish.React

//  Active Patterns are special functions that 
//  extract information from an object so they 
//  can be used in pattern matching.
let (|KeyCode|_|) (e:React.KeyboardEvent) = 
    Some(e.keyCode)

let internal onEnter msg dispatch =
    OnKeyDown <| function 
        | KeyCode(ENTER_KEY) as ev -> ev.target?value <- ""
                                      dispatch msg
        | _ -> ()

let viewInput (model:string) dispatch =
    R.header [ ClassName "header" ] [
        R.h1 [] [ R.str "DC Todos" ]
        R.input [
            ClassName "new-todo"
            Placeholder "What needs to be done?"
            AutoFocus true
            valueOrDefault model
            onEnter Add dispatch
            OnChange (fun (ev:React.FormEvent) -> !!ev.target?value |> UpdateField |> dispatch)
        ]
    ]

//  Define a helper
let internal classList classes =
    ("", classes)
    ||> List.fold (fun complete -> function | (name, true) -> complete + " " + name | _ -> complete)
    |> ClassName

let viewEntry todo dispatch =
  R.li
    [ classList [ ("completed", todo.completed); ("editing", todo.editing) ]]
    [ R.div
        [ ClassName "view" ]
        [ R.input
            [ ClassName "toggle"
              Type "checkbox"
              Checked todo.completed
              OnChange (fun _ -> Check(todo.id, (not todo.completed)) |> dispatch) ]
          R.label
            [ OnDoubleClick (fun _ -> EditingEntry(todo.id, true) |> dispatch) ]
            [ R.str todo.description ]
          R.button
            [ ClassName "destroy"
              OnClick (fun _-> Delete(todo.id) |> dispatch) ]
            []
        ]
      R.input
        [ ClassName "edit"
          valueOrDefault todo.description
          Name "title"
          Id ("todo-" + (string todo.id))
          OnInput (fun ev -> UpdateEntry (todo.id, !!ev.target?value) |> dispatch)
          OnBlur (fun _ -> EditingEntry (todo.id, false) |> dispatch)
          onEnter (EditingEntry (todo.id, false)) dispatch ]
    ]

let viewEntries visibility entries dispatch =
    let isVisible todo =
        match visibility with
        | COMPLETED_TODOS -> todo.completed
        | ACTIVE_TODOS -> not todo.completed
        | _ -> true

    let allCompleted =
        List.forall (fun t -> t.completed) entries

    let cssVisibility =
        if List.isEmpty entries then "hidden" else "visible"

    R.section
      [ ClassName "main"
        Style [ Visibility cssVisibility ]]
      [ R.input
          [ ClassName "toggle-all"
            Type "checkbox"
            Name "toggle"
            Checked allCompleted
            OnChange (fun _ -> CheckAll(not allCompleted) |> dispatch)]
        R.label
          [ HtmlFor "toggle-all" ]
          [ R.str "Mark all as complete" ]
        R.ul
          [ ClassName "todo-list" ]
          (entries
           |> List.filter isVisible
           |> List.map (fun i -> lazyView2 viewEntry i dispatch)) ]

// VIEW CONTROLS AND FOOTER
let visibilitySwap uri visibility actualVisibility dispatch =
  R.li
    [ OnClick (fun _ -> ChangeVisibility visibility |> dispatch) ]
    [ R.a [ Href uri
            classList ["selected", visibility = actualVisibility] ]
          [ R.str visibility ] ]

let viewControlsFilters visibility dispatch =
  R.ul
    [ ClassName "filters" ]
    [ visibilitySwap "#/" ALL_TODOS visibility dispatch
      R.str " "
      visibilitySwap "#/active" ACTIVE_TODOS visibility dispatch
      R.str " "
      visibilitySwap "#/completed" COMPLETED_TODOS visibility dispatch ]

let viewControlsCount entriesLeft =
  let item =
      if entriesLeft = 1 then " item" else " items"

  R.span
      [ ClassName "todo-count" ]
      [ R.strong [] [ R.str (string entriesLeft) ]
        R.str (item + " left") ]

let viewControlsClear entriesCompleted dispatch =
  R.button
    [ ClassName "clear-completed"
      Hidden (entriesCompleted = 0)
      OnClick (fun _ -> DeleteComplete |> dispatch)]
    [ R.str ("Clear completed (" + (string entriesCompleted) + ")") ]

let viewControls visibility entries dispatch =
  let entriesCompleted =
      entries
      |> List.filter (fun t -> t.completed)
      |> List.length

  let entriesLeft =
      List.length entries - entriesCompleted

  R.footer
      [ ClassName "footer"
        Hidden (List.isEmpty entries) ]
      [ lazyView viewControlsCount entriesLeft
        lazyView2 viewControlsFilters visibility dispatch
        lazyView2 viewControlsClear entriesCompleted dispatch ]


let infoFooter =
  R.footer [ ClassName "info" ]
    [ R.p []
        [ R.str "Double-click to edit a todo" ]
      R.p []
        [ R.str "Ported from Elm by "
          R.a [ Href "https://github.com/et1975" ] [ R.str "Eugene Tolmachev" ]]
      R.p []
        [ R.str "Part of "
          R.a [ Href "http://todomvc.com" ] [ R.str "TodoMVC" ]]
    ]

//  In addition to the model, the F#
let view model dispatch =
  R.div
    [ ClassName "todomvc-wrapper"]
    [ R.section
        [ ClassName "todoapp" ]
        [ lazyView2 viewInput model.field dispatch
          lazyView3 viewEntries model.visibility model.entries dispatch
          lazyView3 viewControls model.visibility model.entries dispatch ]
      infoFooter ]




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