
import { ZeroEditor } from '../../../editor/editor';
import MetadataEditor from '../pages/detail/metadata.vue';

const editor = new ZeroEditor(); //('media:edit', '@media.fields.');
editor.resourcePrefix = '@media.fields.';
//editor.blueprintAlias = 'media';

editor.field('name', { label: '@ui.name' }).text();
editor.field('imageMeta.alternativeText', { optional: true, hidden: x => x.imageMeta }).text();
editor.field('caption').textarea();
editor.field('createdDate', { hideLabel: true }).component(MetadataEditor);

export default editor;