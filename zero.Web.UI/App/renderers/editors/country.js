
import Editor from 'zero/core/editor.js';

const editor = new Editor('country', '@country.fields.');

editor.field('name', { label: '@ui.name' }).text(120).required();
//editor.field('alias', { label: '@ui.alias' }).text().required();
editor.field('code').text(2).required();
editor.field('isPreferred').toggle();

export default editor;