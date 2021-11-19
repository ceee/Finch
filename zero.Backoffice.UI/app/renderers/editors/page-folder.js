
import Editor from 'zero/core/editor.ts';

const editor = new Editor('pages.' + __zero.alias.pages.folder, '@page.folder.fields.');

editor.field('isPartOfRoute').toggle();
editor.field('urlAlias').text(40).when(x => x.isPartOfRoute);

export default editor;