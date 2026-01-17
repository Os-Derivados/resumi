<template>
	<UContainer 
		class="w-dvw flex flex-col px-32 py-12 gap-12 bg-gray-50 top-5 left-5">
		
		<UContainer as="header" class="w-full h-24 flex justify-between items-center">
			<p class="text-2xl font-medium font-mono inline-flex">Bom te ver de novo, NomeAleatorio123</p>
			<UserComponent></UserComponent>
		</UContainer>	
		
		<UContainer class="flex justify-center h-12 w-full bg-[url(/assets/images/backgroundhome.png)]">
			<div class="border-[#D8D4D3] border h-full w-auto px-0 block">
				<UContainer class="bg-gray-50 h-full w-45 flex justify-center items-center">
					<p class="font-medium text-sm">meus curriculos</p>
				</UContainer>
			</div>
		</UContainer>

		
		<UMain>
			<UContainer>
				<UModal title="Criar Curriculo">
					<UButton label="Novo Curriculo" icon="i-custom-round-star"></UButton>

					<template #body>
						<UContainer class="pd-12 ">
							<UForm :state="vm.state" :schema="vm.schema" @submit="(e) => vm.requestCreateResume(e)"
								class="flex flex-col gap-8">
								<UFormField label="Titulo do curriculo" name="title" class="text-lg font-medium">
									<UInput v-model="vm.state.title" type="text" class="w-full h-4"/>
								</UFormField>
								
								<UButton type="submit" label="Criar Curriculo" class="self-center flex justify-center items-center
								w-48 h-10 px-4 text-lg font-medium "/>
							</UForm>
						</UContainer>
					</template>
				</UModal>
			</UContainer>
		</UMain>
	</UContainer>
</template>

<script setup lang="ts">
import { CreateResumeViewModel } from '~/views/models/create-resume.vm';

	definePageMeta({
		colorMode: "light"

	})
	const vm = new CreateResumeViewModel()
</script>