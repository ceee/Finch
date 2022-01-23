
import Document from '@tiptap/extension-document'
import Paragraph from '@tiptap/extension-paragraph'
import Text from '@tiptap/extension-text'

import Bold from '@tiptap/extension-bold';
import Code from '@tiptap/extension-code';
import Italic from '@tiptap/extension-italic';
import Strike from '@tiptap/extension-strike';
//import Underline from '@tiptap/ex';
import History from '@tiptap/extension-history';
//import Placeholder from '@tiptap/extension-placeholder';
import HorizontalRule from '@tiptap/extension-horizontal-rule';
import ListItem from '@tiptap/extension-list-item';
import BulletList from '@tiptap/extension-bullet-list';
import OrderedList from '@tiptap/extension-ordered-list';
import Heading from '@tiptap/extension-heading';
import HardBreak from '@tiptap/extension-hard-break';
import Image from '@tiptap/extension-image';
import { ZeroLink, pickLink } from './extensions/link/link';

export default function ()
{
  return {

    removeExtension(name)
    {
      var ext = this.extensions.find(x => x.name === name);
      ext && this.extensions.splice(this.extensions.indexOf(ext), 1);
    },

    removeCommand(alias)
    {
      var cmd = this.commands.find(x => x.alias === alias);
      cmd && this.commands.splice(this.commands.indexOf(cmd), 1);
    },

    extensions: [
      Document,
      Paragraph,
      Text,
      History,
      HardBreak,
      ZeroLink,
      Bold,
      Code,
      Italic,
      Strike,
      //Underline,
      ListItem,
      BulletList,
      OrderedList,
      HorizontalRule,
      Heading.configure({ // TODO
        levels: [2, 3, 4],
      })
    ],

    commands: [
      //{
      //  alias: 'undo',
      //  title: '@rte.undo',
      //  symbol: 'fth-chevron-left',
      //  disabled: cmd => cmd.undoDepth() > 1,
      //  onClick: (ev, cmd) => cmd.undo(ev),
      //},
      //{
      //  alias: 'redo',
      //  title: '@rte.redo',
      //  symbol: 'fth-chevron-right',
      //  disabled: cmd => cmd.redoDepth() > 1,
      //  onClick: (ev, cmd) => cmd.redo(ev)
      //},
      {
        alias: 'bold',
        title: '@rte.bold',
        symbol: 'fth-bold',
        symbolSize: 14,
        isActive: editor => editor.isActive('bold'),
        onClick: editor => editor.chain().focus().toggleBold().run(),
        bubble: true
      },
      {
        alias: 'italic',
        title: '@rte.italic',
        symbol: 'fth-italic',
        symbolSize: 14,
        isActive: editor => editor.isActive('italic'),
        onClick: editor => editor.chain().focus().toggleItalic().run(),
        bubble: true
      },
      {
        alias: 'underline',
        title: '@rte.underline',
        symbol: 'fth-underline',
        symbolSize: 14,
        isActive: editor => editor.isActive('underline'),
        onClick: editor => editor.chain().focus().toggleUnderline().run(),
        bubble: true
      },
      {
        alias: 'strikethrough',
        title: '@rte.strikethrough',
        symbol: 'fth-strikethrough',
        symbolSize: 14,
        isActive: editor => editor.isActive('strike'),
        onClick: editor => editor.chain().focus().toggleStrike().run(),
        bubble: true
      },
      {
        alias: 'code',
        title: '@rte.code',
        symbol: 'fth-code',
        symbolSize: 14,
        isActive: editor => editor.isActive('code'),
        onClick: editor => editor.chain().focus().toggleCode().run(),
        bubble: true
      },
      {
        alias: 'link',
        title: '@rte.link',
        symbol: 'fth-link-2',
        symbolSize: 14,
        isActive: editor => editor.isActive('link'),
        onClick: editor => pickLink(editor)
      },
      {
        alias: 'line',
        title: '@rte.line',
        symbol: 'fth-minus',
        symbolSize: 15,
        onClick: editor => editor.chain().focus().setHorizontalRule().run()
      },
      {
        alias: 'list',
        title: '@rte.list',
        symbol: 'fth-list',
        symbolSize: 14,
        children: [
          {
            alias: 'ulist',
            title: '@rte.ulist',
            isActive: editor => editor.isActive('bullet_list'),
            onClick: editor => editor.chain().focus().toggleBulletList().run()
          },
          {
            alias: 'olist',
            title: '@rte.olist',
            isActive: editor => editor.isActive('ordered_list'),
            onClick: editor => editor.chain().focus().toggleOrderedList().run()
          }
        ]
      },
      {
        alias: 'heading',
        title: '@rte.heading',
        symbol: 'fth-type',
        symbolSize: 14,
        children: [
          {
            alias: 'h2',
            title: '@rte.heading2',
            isActive: editor => editor.isActive('heading', { level: 2}),
            onClick: editor => editor.chain().focus().toggleHeading({ level: 2 }).run()
          },
          {
            alias: 'h3',
            title: '@rte.heading3',
            isActive: editor => editor.isActive('heading', { level: 3 }),
            onClick: editor => editor.chain().focus().toggleHeading({ level: 3 }).run()
          },
          {
            alias: 'h4',
            title: '@rte.heading4',
            isActive: editor => editor.isActive('heading', { level: 4 }),
            onClick: editor => editor.chain().focus().toggleHeading({ level: 4 }).run()
          }
        ]
      }
    ]
  };
};