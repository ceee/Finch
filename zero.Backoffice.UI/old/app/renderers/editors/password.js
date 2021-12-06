
import Editor from 'zero/core/editor.ts';

const editor = new Editor('media', '@changepasswordoverlay.fields.');

editor.field('currentPassword').password().required();
editor.field('newPassword').password().required();
editor.field('confirmNewPassword').password().required();

export default editor;