
import { Mark, markPasteRule, mergeAttributes } from '@tiptap/core';
import { find } from 'linkifyjs';
import { autolink } from '@tiptap/extension-link/src/helpers/autolink';
import { clickHandler } from '@tiptap/extension-link/src/helpers/clickHandler';
import { pasteHandler } from '@tiptap/extension-link/src/helpers/pasteHandler';
import * as overlays from '../../../../services/overlay';


export interface ZeroLinkOptions
{
  /**
   * If enabled, it adds links as you type.
   */
  autolink: boolean,
  /**
   * If enabled, links will be opened on click.
   */
  openOnClick: boolean,
  /**
   * Adds a link to the current selection if the pasted content only contains an url.
   */
  linkOnPaste: boolean,
  /**
   * A list of HTML attributes to be rendered.
   */
  HTMLAttributes: Record<string, any>,
}

declare module '@tiptap/core' {
  interface Commands<ReturnType>
  {
    zerolink: {
      /**
       * Pick a link
       */
      pickLink: () => ReturnType,
      /**
       * Set a link mark from data
       */
      setLinkData: (data: object) => ReturnType,
      /**
       * Set a link mark
       */
      setLink: (attributes: { href: string, target?: string }) => ReturnType,
      /**
       * Toggle a link mark
       */
      toggleLink: (attributes: { href: string, target?: string }) => ReturnType,
      /**
       * Unset a link mark
       */
      unsetLink: () => ReturnType,
    }
  }
}


export async function pickLink(editor)
{
  const result = await overlays.open({
    component: () => import('../../../../modules/links/ui-linkpicker-overlay.vue'),
    display: 'editor',
    model: {
      value: null,
      areas: [],
      allowTitle: false,
      allowTarget: true,
      allowSuffix: true
    }
  });

  if (result.eventType == 'confirm')
  {
    return editor.chain().focus().setLinkData(result.value).run();
  }
}


export const ZeroLink = Mark.create<ZeroLinkOptions>({
  name: 'zerolink',

  priority: 1000,

  keepOnSplit: false,

  inclusive()
  {
    return this.options.autolink
  },

  addOptions()
  {
    return {
      openOnClick: true,
      linkOnPaste: true,
      autolink: true,
      HTMLAttributes: {
        target: '_self',
        rel: 'noopener noreferrer nofollow',
      },
    }
  },

  addAttributes()
  {
    return {
      href: {
        default: null,
      },
      'zero-link': {
        default: null
      },
      target: {
        default: this.options.HTMLAttributes.target,
      },
      title: {
        default: null
      }
    }
  },

  parseHTML()
  {
    return [
      { tag: 'a[href]' },
      { tag: 'a[zero-link]' }
    ]
  },

  renderHTML({ HTMLAttributes })
  {
    return [
      'a',
      mergeAttributes(this.options.HTMLAttributes, HTMLAttributes),
      0,
    ]
  },

  addCommands()
  {
    return {

      setLinkData: data => ({ chain }) =>
      {
        let attributes = {};

        if (data.target !== 'default')
        {
          attributes.target = data.target;
        }

        if (data.title)
        {
          attributes.title = data.title;
        }

        let link = {
          area: data.area
        };

        if (data.values)
        {
          link.values = data.values;
        }

        if (data.urlSuffix)
        {
          link.suffix = data.urlSuffix;
        }

        attributes['zero-link'] = JSON.stringify(link);

        return chain()
          .setMark(this.name, attributes)
          .setMeta('preventAutolink', true)
          .run()
      },

      setLink: attributes => ({ chain }) =>
      {
        return chain()
          .setMark(this.name, attributes)
          .setMeta('preventAutolink', true)
          .run()
      },

      toggleLink: attributes => ({ chain }) =>
      {
        return chain()
          .toggleMark(this.name, attributes, { extendEmptyMarkRange: true })
          .setMeta('preventAutolink', true)
          .run()
      },

      unsetLink: () => ({ chain }) =>
      {
        return chain()
          .unsetMark(this.name, { extendEmptyMarkRange: true })
          .setMeta('preventAutolink', true)
          .run()
      },
    }
  },

  addPasteRules()
  {
    return [
      markPasteRule({
        find: text => find(text)
          .filter(link => link.isLink)
          .map(link => ({
            text: link.value,
            index: link.start,
            data: link,
          })),
        type: this.type,
        getAttributes: match => ({
          href: match.data?.href,
        }),
      }),
    ]
  },

  addProseMirrorPlugins()
  {
    const plugins = []

    if (this.options.autolink)
    {
      plugins.push(autolink({
        type: this.type,
      }))
    }

    if (this.options.openOnClick)
    {
      plugins.push(clickHandler({
        type: this.type,
      }))
    }

    if (this.options.linkOnPaste)
    {
      plugins.push(pasteHandler({
        editor: this.editor,
        type: this.type,
      }))
    }

    return plugins
  },
})