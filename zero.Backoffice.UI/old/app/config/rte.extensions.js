
import { Node, Extension, Plugin } from 'tiptap';
import { chainCommands, exitCode } from 'prosemirror-commands';

export class HardBreak extends Node
{
  get name()
  {
    return 'br'
  }

  get schema()
  {
    return {
      inline: true,
      group: 'inline',
      selectable: false,
      parseDOM: [
        { tag: 'br' },
      ],
      toDOM: () => ['br'],
    }
  }

  keys({ type })
  {
    const command = chainCommands(exitCode, (state, dispatch) =>
    {
      dispatch(state.tr.replaceSelectionWith(type.create()).scrollIntoView())
      return true
    })
    return {
      'Enter': command
    }
  }
}


export class MaxSize extends Extension
{
  get name()
  {
    return 'maxSize'
  }

  get defaultOptions()
  {
    return {
      maxSize: null
    }
  }

  get plugins()
  {
    return [
      new Plugin({
        appendTransaction: (transactions, oldState, newState) =>
        {
          const max = this.options.maxSize;
          const oldLength = oldState.doc.content.size;
          const newLength = newState.doc.content.size;

          if (max && newLength > max && newLength > oldLength)
          {
            let newTr = newState.tr;
            newTr.insertText('', max + 1, newLength);
            return newTr;
          }
        }
      })
    ]
  }
}