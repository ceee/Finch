
//import { HardBreak } from './rte.extensions.js';
import { Bold, Code, Italic, Link, Strike, Underline, History, Placeholder, HorizontalRule, ListItem, BulletList, OrderedList, Heading, HardBreak } from 'tiptap-extensions';

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
      new History(),
      new HardBreak(),
      new Link({
        target: '_blank'
      }), // TODO
      new Bold(),
      new Code(),
      new Italic(),
      new Strike(),
      new Underline(),
      new ListItem(),
      new BulletList(),
      new OrderedList(),
      new HorizontalRule(),
      new Heading({ // TODO
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
        isActive: active => active.bold(),
        onClick: (ev, cmd) => cmd.bold(ev),
        bubble: true
      },
      {
        alias: 'italic',
        title: '@rte.italic',
        symbol: 'fth-italic',
        symbolSize: 14,
        isActive: active => active.italic(),
        onClick: (ev, cmd) => cmd.italic(ev),
        bubble: true
      },
      {
        alias: 'underline',
        title: '@rte.underline',
        symbol: 'fth-underline',
        symbolSize: 14,
        isActive: active => active.underline(),
        onClick: (ev, cmd) => cmd.underline(ev),
        bubble: true
      },
      //{
      //  alias: 'strikethrough',
      //  title: '@rte.strike',
      //  symbol: 'fth-zap-off',
      //  symbolSize: 14,
      //  isActive: active => active.strike(),
      //  onClick: (ev, cmd) => cmd.strike(ev)
      //},
      {
        alias: 'code',
        title: '@rte.code',
        symbol: 'fth-code',
        symbolSize: 14,
        isActive: active => active.code(),
        onClick: (ev, cmd) => cmd.code(ev),
        bubble: true
      },
      {
        alias: 'line',
        title: '@rte.line',
        symbol: 'fth-minus',
        symbolSize: 15,
        onClick: (ev, cmd) => cmd.horizontal_rule(ev)
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
            isActive: active => active.bullet_list(),
            onClick: (ev, cmd) => cmd.bullet_list(ev)
          },
          {
            alias: 'olist',
            title: '@rte.olist',
            isActive: active => active.ordered_list(),
            onClick: (ev, cmd) => cmd.ordered_list(ev)
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
            isActive: active => active.heading({ level: 2 }),
            onClick: (ev, cmd) => cmd.heading({ level: 2 })
          },
          {
            alias: 'h3',
            title: '@rte.heading3',
            isActive: active => active.heading({ level: 3 }),
            onClick: (ev, cmd) => cmd.heading({ level: 3 })
          },
          {
            alias: 'h4',
            title: '@rte.heading4',
            isActive: active => active.heading({ level: 4 }),
            onClick: (ev, cmd) => cmd.heading({ level: 4 })
          }
        ]
      }
    ]
  };
};