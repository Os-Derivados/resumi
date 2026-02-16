<template>
    <UContainer class="w-6xl">
        <header class="flex justify-between">
			<UserComponent />
		</header>

		<ResumeForm />
    </UContainer>
</template>

<script setup lang="ts">
import ResumeForm from '~/views/components/resume-form.vue';
import UserComponent from '~/views/components/user-component.vue';

definePageMeta({
	colorMode: "light"
})
</script>