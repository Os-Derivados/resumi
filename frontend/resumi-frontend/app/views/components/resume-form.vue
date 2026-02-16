<template>
	<UForm :schema="vm.schema" :state="vm.state"  @submit="(e) => e.preventDefault()">
		<UFormField label="Owner Name" name="ownerName" class="w-full">
			<UInput v-model="vm.state.ownerName" class="w-full" />
		</UFormField>

		<UFormField label="Email" name="email" class="w-full">
			<UInput v-model="vm.state.email" class="w-full" />
		</UFormField>

		<UFormField label="Keyword" name="keyword" class="w-full">
			<UInput v-model="vm.state.keyword" class="w-full" />
		</UFormField>

		<UFormField label="Location" name="location" class="w-full">
			<UInput v-model="vm.state.location" class="w-full" />
		</UFormField>

		<UFormField label="Phone Number" name="phoneNumber" class="w-full">
			<UInput v-model="vm.state.phoneNumber" class="w-full" />
		</UFormField>
	</UForm>
</template>

<script setup lang="ts">
import { ResumeFormViewModel } from '../models/resume-form.vm';

const route = useRoute()
const resumeId = Number(route.params.id)

const vm = new ResumeFormViewModel(resumeId)
</script>