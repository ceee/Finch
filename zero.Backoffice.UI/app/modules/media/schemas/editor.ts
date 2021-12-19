
import Editor from '../../../editor/editor';
import { getFilesize, formatDate } from '../../../utils';
import MetadataEditor from '../pages/detail/metadata.vue';

const editor = new Editor('media:edit', '@media.fields.');
editor.blueprintAlias = 'media';

editor.field('name', { label: '@ui.name' }).text().required();
//editor.field('source').custom(MediaUpload).required();
editor.field('imageMeta.alternativeText').when(x => x.imageMeta).text();
editor.field('caption').textarea();

editor.field('createdDate', { hideLabel: true }).custom(MetadataEditor);
//editor.field('size').vertical(false).output(x => getFilesize(x, 2));
//editor.field('createdDate', { label: '@ui.createdDate' }).vertical(false).output(x => formatDate(x));
//editor.field('lastModifiedDate', { label: '@ui.modifiedDate' }).vertical(false).output(x => formatDate(x));

//editor.field('imageMeta.width').vertical(false).output((x, entity) => entity.imageMeta.width + ' × ' + entity.imageMeta.height);
////meta.field('imageMeta.imageTakenDate').vertical(false).output(x => formatDate(x));
////meta.field('imageMeta.frames').vertical(false).output();
//editor.field('imageMeta.dpi').vertical(false).output();

 //<ui-property v-if="modelValue.lastModifiedDate" field="lastModifiedDate" label="@ui.modifiedDate">
 //         <ui-date v-model="modelValue.lastModifiedDate" />
 //       </ui-property>
 //       <ui-property label="@ui.createdDate" field="createdDate">
 //         <ui-date v-model="modelValue.createdDate" />
 //       </ui-property>
 //       <ui-property label="@ui.entityfields.alias" field="alias">
 //         {{modelValue.alias}}
 //       </ui-property>
 //       <ui-property label="@ui.entityfields.sort" field="sort">
 //         {{modelValue.sort}}
 //       </ui-property>
 //       <ui-property v-if="modelValue.key" label="@ui.entityfields.key" field="key">
 //         {{modelValue.key}}
 //       </ui-property>

export default editor;