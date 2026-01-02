<template>
	<main class="w-full min-h-screen px-4 sm:px-8 md:px-16 lg:px-24 py-8 sm:py-12 md:py-16 grid items-center">
		<UForm :schema="vm.schema" :state="vm.state" class="rounded-2xl shadow-md p-6 sm:p-8 max-w-md md:max-w-lg"
			@submit="(e) => vm.requestLoginAsync(e)">
			<fieldset class="space-y-4  w-full flex flex-col items-center mb-8">
				<legend class="text-lg mb-8 font-medium">Iniciar Sessão</legend>

				<UFormField label="Email" name="email" class="w-full">
					<UInput v-model="vm.state.email" class="w-full" />
				</UFormField>

				<UFormField label="Senha" name="password" class="w-full">
					<UInput v-model="vm.state.password" type="password" class="w-full" />
				</UFormField>

				<p class="text-sm sm:text-base">
					Não possui conta? Acesse nossa
					<ULink to="/signup" class="text-primary">página de cadastro</ULink>
				</p>
			</fieldset>

			<UButton type="submit" size="xl" class="block mx-auto">
				Login
			</UButton>
		</UForm>
	</main>
</template>

<script setup lang="ts">
import { LoginViewModel } from '~/views/models/login.vm';

definePageMeta({
	colorMode: 'light'
})

const vm = new LoginViewModel()
</script>