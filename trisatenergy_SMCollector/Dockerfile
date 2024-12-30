FROM mcr.microsoft.com/dotnet/runtime:9.0

WORKDIR /app

ARG REPO_SOURCE=https://github.com/TriSAT-Collective/smart-meter-collector
ARG DESCRIPTION="collects smart-meter data"
ARG ARTIFACT_PATH=dist/

ARG ENTRYPOINT=/app/trisatenergy_SMCollector
ENV ENTRYPOINT=$ENTRYPOINT

LABEL org.opencontainers.image.source=$REPO_SOURCE
LABEL org.opencontainers.image.description=$DESCRIPTION

COPY $ARTIFACT_PATH /app

RUN chmod +x $ENTRYPOINT

ENTRYPOINT $ENTRYPOINT
CMD []
