<template>
	<UForm class="w-full shadow py-8 px-16 my-16" :schema="vm.schema" :state="vm.state"
		@submit="(e) => e.preventDefault()">
		<UFormField label="Título" name="ownerName" class="w-full">
			<UInput v-model="vm.state.ownerName" :variant="vm.getVariant('ownerName')"
				@focus="vm.setFocus('ownerName', true)" @blur="vm.setFocus('ownerName', false)" class="w-full" :ui="{
					base: 'text-2xl font-bold mb-4',
				}" />
		</UFormField>

		<ul class="flex gap-8 my-4">
			<li>
				<UFormField label="E-mail" name="email" class="w-full">
					<UInput v-model="vm.state.email" :variant="vm.getVariant('email')"
						@focus="vm.setFocus('email', true)" @blur="vm.setFocus('email', false)" class="w-full" />
				</UFormField>
			</li>

			<li>
				<UFormField label="Cidade" name="location" class="w-full">
					<UInput v-model="vm.state.location" :variant="vm.getVariant('location')"
						@focus="vm.setFocus('location', true)" @blur="vm.setFocus('location', false)" class="w-full" />
				</UFormField>
			</li>

			<li>
				<UFormField label="Telefone" name="phoneNumber" class="w-full">
					<UInput v-model="vm.state.phoneNumber" :variant="vm.getVariant('phoneNumber')"
						@focus="vm.setFocus('phoneNumber', true)" @blur="vm.setFocus('phoneNumber', false)"
						class="w-full" />
				</UFormField>
			</li>
		</ul>

		<UFormField label="Sobre Mim" name="description" class="w-full mb-4">
			<UInput v-model="vm.state.description" :variant="vm.getVariant('description')"
				@focus="vm.setFocus('description', true)" @blur="vm.setFocus('description', false)" class="w-full" />
		</UFormField>

		<UFormField label="Palavras-chave" name="keyword" class="w-full">
			<UInput v-model="vm.state.keyword" :variant="vm.getVariant('keyword')" @focus="vm.setFocus('keyword', true)"
				@blur="vm.setFocus('keyword', false)" class="w-full" />
		</UFormField>

		<article class="my-8">
			<h2 class="text-lg font-semibold">Experiências</h2>

			<p>Nenhuma experiência adicionada.</p>

			<UButton label="Adicionar experiência" icon="i-lucide-plus" class="mt-4" />
		</article>

		<article class="my-8">
			<h2 class="text-lg font-semibold">Formação Acadêmica</h2>

			<p>Nenhuma formação acadêmica adicionada.</p>

			<UButton label="Adicionar formação acadêmica" icon="i-lucide-plus" class="mt-4" />
		</article>

		<article class="my-8">
			<h2 class="text-lg font-semibold">Atividades Extracurriculares</h2>

			<p>Nenhuma atividade extracurricular adicionada.</p>

			<UButton label="Adicionar atividade extracurricular" icon="i-lucide-plus" class="mt-4" />
		</article>
	</UForm>
</template>

<script setup lang="ts">
import { ResumeFormViewModel } from '../models/resume-form.vm';

const route = useRoute()
const resumeId = Number(route.params.id)

const vm = new ResumeFormViewModel(resumeId)
</script>