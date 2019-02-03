module App.Views

// rendering views with React
module R = Fable.Helpers.React
open Fable.Core.JsInterop
open Fable.Helpers.React.Props
open Elmish.React

open Fable.Core
open Fable.Import
open Elmish

//  Notice the F# program begins at the bottom

let [<Literal>] ESC_KEY = 27.
let [<Literal>] ENTER_KEY = 13.

open App.Model
open App.Update
open App.Storage

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