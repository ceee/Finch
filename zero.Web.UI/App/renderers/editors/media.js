
import Editor from 'zero/core/editor.js';
import MediaUpload from 'zero/pages/media/upload.vue';

const editor = new Editor('media', '@media.fields.');

editor.field('source').custom(MediaUpload).required();
editor.field('name', { label: '@ui.name' }).text().required();
editor.field('alternativeText').text();
editor.field('caption').textarea();

export default editor;