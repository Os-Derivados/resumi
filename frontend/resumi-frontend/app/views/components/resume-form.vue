<template>
	hi mom
</template>

<script setup lang="ts">
import { ResumeFormViewModel } from '../models/resume-form.vm';

const route = useRoute()
const vm = new ResumeFormViewModel(Number(route.params['id'] as unknown))


</script>