
import Editor from 'zero/core/editor.js';

const editor = new Editor('language', '@language.fields.');

editor.field('name', { label: '@ui.name' }).text(60).required();
editor.field('code').text(10).required();
editor.field('inheritedLanguageId').languagePicker();
editor.field('isDefault').toggle();
editor.field('isOptions').toggle();

export default editor;