
import Editor from 'zero/core/editor.ts';

const editor = new Editor('translation', '@translation.fields.');
editor.blueprintAlias = 'translation';

editor.fieldset(set =>
{
  set.field('key').text(300).required();
  set.field('display').state([
    { label: '@translation.display.text', value: 'text' },
    { label: '@translation.display.html', value: 'html' }
  ]);
});

editor.field('value').rte().required();

export default editor;