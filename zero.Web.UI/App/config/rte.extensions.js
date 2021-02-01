
import { Node } from 'tiptap';
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