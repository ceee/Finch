<template>
  <a v-if="isExternalLink"
     v-bind="attrs"
     class="ui-link"
     :class="classes"
     target="_blank"
     rel="noopener noreferrer"
     :href="to"
     :tabindex="disabled ? -1 : undefined"
     :aria-disabled="disabled">
    <slot />
  </a>
  <a v-else
     v-bind="attrs"
     class="ui-link"
     :class="classes"
     :href="href"
     :tabindex="disabled ? -1 : undefined"
     :aria-disabled="disabled"
     @click="navigate">
    <slot />
  </a>
</template>

<script lang="ts">
  import { RouterLinkProps, RouteLocationRaw, START_LOCATION, useLink, useRoute } from 'vue-router';
  import { computed, defineComponent, toRefs, PropType } from 'vue';

  export default defineComponent({
    name: 'ui-link',

    props: {
      to: {
        type: [String, Object] as PropType<RouteLocationRaw>,
        required: true,
      },
      replace: Boolean,
      custom: Boolean,
      ariaCurrentValue: {
        type: String as PropType<RouterLinkProps['ariaCurrentValue']>,
        default: 'page',
      },
      disabled: Boolean
    },

    setup(props, { attrs })
    {
      const { replace, to, disabled } = toRefs(props);

      const isExternalLink = computed(
        () => typeof to.value === 'string' && to.value.startsWith('http')
      );

      const currentRoute = useRoute();

      const { route, href, isActive, isExactActive, navigate } = useLink({
        to: computed(() => (isExternalLink.value ? START_LOCATION : to.value)),
        replace,
      });

      const classes = computed(() => ({
        'is-active': isActive.value || currentRoute.path.startsWith(route.value.path),
        'is-active-exact': isExactActive.value || currentRoute.path === route.value.path
      }));

      return { attrs, isExternalLink, href, navigate, classes, disabled };
    },
  })
</script>

<style lang="scss">
  .ui-link[aria-disabled]
  {
    pointer-events: none;
  }
</style>