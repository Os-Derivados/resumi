<template>
	<main class="w-full min-h-screen px-4 sm:px-8 md:px-16 lg:px-24 py-8 sm:py-12 md:py-16">
		<header class="text-center w-full mb-12 sm:mb-16">
			<h1 class="text-3xl sm:text-4xl mb-4">Comece Já</h1>

			<p class="text-base sm:text-lg">
				Registre-se e tenha acesso à melhor plataforma de gestão de currículos.
			</p>
		</header>

		<section
			class="grid grid-cols-1 grid-rows-2 place-items-center md:grid-cols-2 md:grid-rows-1 gap-16 md:gap-32 justify-center items-center">
			<UForm :schema="vm.schema" :state="vm.state"
				class="rounded-2xl shadow-md p-6 sm:p-8 max-w-md md:max-w-lg md:justify-self-end"
				@submit="(e) => vm.requestUserCreationAsync(e)">
				<fieldset class="space-y-4  w-full flex flex-col items-center mb-8 ">
					<legend class="text-lg mb-8 font-medium">Informações Básicas</legend>

					<UFormField label="Nome Completo" name="fullName" class="w-full">
						<UInput v-model="vm.state.fullName" class="w-full" />
					</UFormField>

					<UFormField label="Email" name="email" class="w-full">
						<UInput v-model="vm.state.email" class="w-full" />
					</UFormField>

					<UFormField label="Telefone" name="phoneNumber" class="w-full">
						<UInput v-model="vm.state.phoneNumber" class="w-full" />
					</UFormField>

					<UFormField label="Senha" name="password" class="w-full">
						<UInput v-model="vm.state.password" type="password" class="w-full" />
					</UFormField>

					<p class="text-sm sm:text-base">
						Já possui conta? Acesse nossa
						<ULink to="/login" class="text-primary">página de login</ULink>
					</p>
				</fieldset>

				<UButton type="submit" size="xl" class="block mx-auto">
					Registrar
				</UButton>
			</UForm>

			<aside class="w-full max-w-md md:max-w-lg text-center md:text-left md:justify-self-start">
				<header>
					<img src="~/assets/images/signup.svg" alt="Signup illustration"
						class="w-full h-auto max-w-xs md:max-w-sm mx-auto md:mx-0" />
					<p class="text-base sm:text-lg mb-6 mt-4">
						Na Resumi, escolhemos o melhor pra você
					</p>
				</header>

				<p class="text-sm sm:text-base leading-relaxed">
					Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto reiciendis quod neque,
					corporis autem a voluptates tempora qui esse quos est id beatae! Incidunt vero culpa,
					voluptates ab natus recusandae.
				</p>
			</aside>
		</section>
	</main>
</template>

<script setup lang="ts">
import { SignupViewModel } from '~/views/models/signup.vm'

definePageMeta({
	colorMode: 'light'
})

const vm = new SignupViewModel()
</script>
