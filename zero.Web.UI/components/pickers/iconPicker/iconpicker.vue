<template>
  <div v-if="output" class="ui-iconpicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <ui-select-button :icon="previewIcon" label="@ui.icon" :description="buttonDescription" @click="pick" :disabled="disabled" />
  </div>
</template>


<script>
  import PickIconOverlay from './overlay.vue';
  import Overlay from '@zero/services/overlay.js';
  import { extend as _extend } from 'underscore';

  const FEATHER_ICONS = ['fth-alert-octagon','fth-alert-circle','fth-activity','fth-alert-triangle','fth-align-center','fth-airplay','fth-align-justify','fth-align-left','fth-align-right','fth-arrow-down-left','fth-arrow-down-right','fth-anchor','fth-aperture','fth-arrow-left','fth-arrow-right','fth-arrow-down','fth-arrow-up-left','fth-arrow-up-right','fth-arrow-up','fth-award','fth-bar-chart','fth-at-sign','fth-bar-chart-2','fth-battery-charging','fth-bell-off','fth-battery','fth-bluetooth','fth-bell','fth-book','fth-briefcase','fth-camera-off','fth-calendar','fth-bookmark','fth-box','fth-camera','fth-check-circle','fth-check','fth-check-square','fth-cast','fth-chevron-down','fth-chevron-left','fth-chevron-right','fth-chevron-up','fth-chevrons-down','fth-chevrons-right','fth-chevrons-up','fth-chevrons-left','fth-circle','fth-clipboard','fth-chrome','fth-clock','fth-cloud-lightning','fth-cloud-drizzle','fth-cloud-rain','fth-cloud-off','fth-codepen','fth-cloud-snow','fth-compass','fth-copy','fth-corner-down-right','fth-corner-down-left','fth-corner-left-down','fth-corner-left-up','fth-corner-up-left','fth-corner-up-right','fth-corner-right-down','fth-corner-right-up','fth-cpu','fth-credit-card','fth-crosshair','fth-disc','fth-delete','fth-download-cloud','fth-download','fth-droplet','fth-edit-2','fth-edit','fth-edit-1','fth-external-link','fth-eye','fth-feather','fth-facebook','fth-file-minus','fth-eye-off','fth-fast-forward','fth-file-text','fth-film','fth-file','fth-file-plus','fth-folder','fth-filter','fth-flag','fth-globe','fth-grid','fth-heart','fth-home','fth-github','fth-image','fth-inbox','fth-layers','fth-info','fth-instagram','fth-layout','fth-link-2','fth-life-buoy','fth-link','fth-log-in','fth-list','fth-lock','fth-log-out','fth-loader','fth-mail','fth-maximize-2','fth-map','fth-map-pin','fth-menu','fth-message-circle','fth-message-square','fth-minimize-2','fth-mic-off','fth-minus-circle','fth-mic','fth-minus-square','fth-minus','fth-moon','fth-monitor','fth-more-vertical','fth-more-horizontal','fth-move','fth-music','fth-navigation-2','fth-navigation','fth-octagon','fth-package','fth-pause-circle','fth-pause','fth-percent','fth-phone-call','fth-phone-forwarded','fth-phone-missed','fth-phone-off','fth-phone-incoming','fth-phone','fth-phone-outgoing','fth-pie-chart','fth-play-circle','fth-play','fth-plus-square','fth-plus-circle','fth-plus','fth-pocket','fth-printer','fth-power','fth-radio','fth-repeat','fth-refresh-ccw','fth-rewind','fth-rotate-ccw','fth-refresh-cw','fth-rotate-cw','fth-save','fth-search','fth-server','fth-scissors','fth-share-2','fth-share','fth-shield','fth-settings','fth-skip-back','fth-shuffle','fth-sidebar','fth-skip-forward','fth-slack','fth-slash','fth-smartphone','fth-square','fth-speaker','fth-star','fth-stop-circle','fth-sun','fth-sunrise','fth-tablet','fth-tag','fth-sunset','fth-target','fth-thermometer','fth-thumbs-up','fth-thumbs-down','fth-toggle-left','fth-toggle-right','fth-trash-2','fth-trash','fth-trending-up','fth-trending-down','fth-triangle','fth-type','fth-twitter','fth-upload','fth-umbrella','fth-upload-cloud','fth-unlock','fth-user-check','fth-user-minus','fth-user-plus','fth-user-x','fth-user','fth-users','fth-video-off','fth-video','fth-voicemail','fth-volume-x','fth-volume-2','fth-volume-1','fth-volume','fth-watch','fth-wifi','fth-x-square','fth-wind','fth-x','fth-x-circle','fth-zap','fth-zoom-in','fth-zoom-out','fth-command','fth-cloud','fth-hash','fth-headphones','fth-underline','fth-italic','fth-bold','fth-crop','fth-help-circle','fth-paperclip','fth-shopping-cart','fth-tv','fth-wifi-off','fth-minimize','fth-maximize','fth-gitlab','fth-sliders','fth-star-on','fth-heart-on','fth-archive','fth-arrow-down-circle','fth-arrow-up-circle','fth-arrow-left-circle','fth-arrow-right-circle','fth-bar-chart-line-','fth-bar-chart-line','fth-book-open','fth-code','fth-database','fth-dollar-sign','fth-folder-plus','fth-gift','fth-folder-minus','fth-git-commit','fth-git-branch','fth-git-pull-request','fth-git-merge','fth-linkedin','fth-hard-drive','fth-more-vertical','fth-more-horizontal','fth-rss','fth-send','fth-shield-off','fth-shopping-bag','fth-terminal','fth-truck','fth-zap-off','fth-youtube','fth-google'];

  export default {
    name: 'uiIconpicker',

    emits: ['change', 'input'],

    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      colors: {
        type: Boolean,
        default: false
      },
      output: {
        type: Boolean,
        default: true
      },
      options: {
        type: Object,
        default: () =>
        {
          return {

          };
        }
      }
    },

    computed: {
      buttonDescription()
      {
        return this.value ? this.value.split(' ')[0] : '@ui.icon_select';
      },
      previewIcon()
      {
        return this.value || 'fth-plus';
      }
    },

    methods: {

      onChange(value)
      {
        this.$emit('change', value);
        this.$emit('input', value);
        // TODO this does not trigger the forms dirty flag
      },

      pick()
      {
        if (this.disabled)
        {
          return;
        }

        let options = _extend({
          title: '@iconpicker.title',
          closeLabel: '@ui.close',
          component: PickIconOverlay,
          display: 'editor',
          items: FEATHER_ICONS,
          model: this.value,
          colors: this.colors,
        }, typeof this.options === 'object' ? this.options : {});

        return Overlay.open(options).then(value =>
        {
          this.onChange(value);
          //this.$refs.input.value = value;
        });
      }
      
    }
  }
</script>