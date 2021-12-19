
import Editor from '../../../editor/editor';
import { getFilesize, formatDate } from '../../../utils';
import MetadataEditor from '../pages/detail/metadata.vue';

const editor = new Editor('media:edit', '@media.fields.');
editor.blueprintAlias = 'media';

editor.field('name', { label: '@ui.name' }).text().required();
editor.field('imageMeta.alternativeText').when(x => x.imageMeta).text();
editor.field('caption').textarea();
editor.field('createdDate', { hideLabel: true }).custom(MetadataEditor);

export default editor;