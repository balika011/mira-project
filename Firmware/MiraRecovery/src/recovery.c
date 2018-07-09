#include <oni/utils/memory/install.h>
#include <oni/init/initparams.h>
#include "device/device.h"

void* recovery_entry(void* args)
{
	struct initparams_t userParams;
	userParams.entrypoint = recovery_init_device;
	userParams.process = NULL;
	userParams.payloadBase = 0x926200000;
	userParams.payloadSize = 0x80000;

	SelfElevateAndRun(&userParams);

	return NULL;
}