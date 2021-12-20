
import { defineStore } from 'pinia';
import { Component } from 'vue';

export type EditorState = {
  fieldTypes: Record<string, Component>
};

export const useEditorStore = defineStore('zero.editor', {
  state: () => ({
    fieldTypes: {}
  } as EditorState),

  actions: {
    
  }
});